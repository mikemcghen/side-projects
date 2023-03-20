using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using TEbucksServer.DAO;
using TEbucksServer.DTO;
using TEbucksServer.Models;
using TEbucksServer.Services;
using TEbucksServer.Utility;
using TEBucksServer.DAO; 
using TEBucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/transfers")]
    [ApiController]
    [Authorize]
    public class TransfersController : ControllerBase
    {
        private ITransferDao transferDao;
        private IAccountDao accountDao;
        private ITransferStatusDao transferStatusDao;
        private IUserDao userDao;
        private ITxLog TxLog;

        public TransfersController(ITransferDao _transferDao, IAccountDao _accountDao, ITransferStatusDao _transferStatusDao, IUserDao _userDao, ITxLog _TxLog)
        {
            this.transferDao = _transferDao;
            this.accountDao = _accountDao;
            this.transferStatusDao = _transferStatusDao;
            this.userDao = _userDao;
            this.TxLog = _TxLog;
        }

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransferById(int id)
        {
            try
            {
                Transfer transfer = transferDao.GetTransferByTransferId(id);
                if (transfer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(transfer);
                }
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Transfer>CreateTransfer(NewTransferDto newTransferDto)
        {
            try
            {
                if(newTransferDto.Amount <= 0)
                {
                    return BadRequest("Amount must be greater than zero.");
                }
                else if(newTransferDto.Account_From == newTransferDto.Account_To)
                {
                    return BadRequest("Can't transfer to the same account");
                }

                //send Transfer
                if(newTransferDto.Transfer_Type_Id == 2)
                {
                    string username = User.Identity.Name;
                    int userId = userDao.GetUser(username).UserId;

                    // Checking sender balance by sender account_id
                    Account senderAccount = accountDao.GetAccountById(newTransferDto.Account_From);
                    if(senderAccount.User_Id != userId)
                    {
                        BasicLogger.Log("Unauthorized access to CreateTransfer");
                        return Unauthorized();
                    }
                    if(senderAccount.Balance < newTransferDto.Amount)
                    {
                        // BasicLogger.Log($"Overdraft attempt by: {User.Identity.Name}. Attempted Transfer Amount: {newTransferDto.Amount:C}");
                        TxLog newLog = new TxLog()
                        {
                            Description = "Overdraft Attempted",
                            Username_From = $"{User.Identity.Name}",
                            Username_To = $"{transferDao.GetAccountToUsername(newTransferDto.Account_To)}",
                            Amount = newTransferDto.Amount
                        };
                        TxLog.AddLog(newLog);
                        return BadRequest("Insufficient balance");
                    }

                    //Checking receiver balance by receiver account_id
                    Account receiverAccount = accountDao.GetAccountById(newTransferDto.Account_To);

                    //Check if receiver is registered or not
                    if(receiverAccount == null)
                    {
                        throw new Exception("Transfer to the registered user only");
                    }

                    //Checking if user transfer to won account
                    if(senderAccount.User_Id == receiverAccount.User_Id)
                    {
                        return BadRequest("You cannot transfer to yourself");
                    }

                    //Create a send transfer
                    Transfer sendTransfer = new Transfer()
                    {
                        Transfer_Type_Id = newTransferDto.Transfer_Type_Id,
                        Account_From = newTransferDto.Account_From,
                        Account_To = newTransferDto.Account_To,
                        Amount = newTransferDto.Amount,
                        Transfer_Status_Id = 2 //For send initial status is "Approved" by default
                    };
                    transferDao.InsertTransfer(sendTransfer);

                    senderAccount.Balance -= newTransferDto.Amount;
                    receiverAccount.Balance += newTransferDto.Amount;

                    bool senderResult = accountDao.UpdateAccount(senderAccount.Account_Id, senderAccount.Balance);
                    bool receiverResult = accountDao.UpdateAccount(receiverAccount.Account_Id, receiverAccount.Balance);

                    // Log transfer of $1,000 or more
                    if (newTransferDto.Amount >= 1000)
                    {
                        //BasicLogger.Log($"Transfer by {User.Identity.Name}. Transfer Amount: {newTransferDto.Amount:C} Transfer To: {newTransferDto.Account_To}");
                        TxLog newLog = new TxLog()
                        {
                            Description = "Transfer made is greater than $1000",
                            Username_From = $"{User.Identity.Name}",
                            Username_To = $"{transferDao.GetAccountToUsername(newTransferDto.Account_To)}",
                            Amount = newTransferDto.Amount
                        };
                        TxLog.AddLog(newLog);
                    }
                    return Created("api/transfers/" + sendTransfer.Transfer_Id, sendTransfer);
                }

                //Request Transfer
                else if(newTransferDto.Transfer_Type_Id == 1)
                {
                    Account requestedAccount = accountDao.GetAccountById(newTransferDto.Account_From);

                    if(requestedAccount == null)
                    {
                        throw new Exception("Transfer to the registered user only");
                    }
                    Transfer requestTransfer = new Transfer()
                    {
                        Transfer_Type_Id = newTransferDto.Transfer_Type_Id,
                        Account_From = newTransferDto.Account_From,
                        Account_To = newTransferDto.Account_To,
                        Amount = newTransferDto.Amount,
                        Transfer_Status_Id = 1 //For request initial status is "Pending by default
                    };
                    transferDao.InsertTransfer(requestTransfer);
                    return Created("api/transfers/" + requestTransfer.Transfer_Id, requestTransfer);
                }
                else
                {
                    return BadRequest("Invalid Transfer type");
                }

            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPut("{id}/status")]
        public ActionResult<Transfer> StatusUpdate(int id, TransferStatusUpdateDto updateStatus)
        {
            //check if the transfer status is valid
            string newTransferStatus = updateStatus.TransferStatus;
            List<string> validStatus = new List<string> { "Pending", "Approved", "Rejected" };
            if (!validStatus.Contains(newTransferStatus))
            {
                return NotFound("Not valid status");
            }

            Transfer transfer = transferDao.GetTransferByTransferId(id);
            Account senderAccount = accountDao.GetAccountById(transfer.Account_From);
            Account receiverAccount = accountDao.GetAccountById(transfer.Account_To);

            //Check if the user is authorize to update the transfer
            if ((senderAccount.Account_Id != transfer.Account_From) && (receiverAccount.Account_Id != transfer.Account_To))
            {
                return Unauthorized();
            }

            bool updateResult = transferStatusDao.UpdateTransferStatus(transfer.Transfer_Id, updateStatus.TransferStatus);

            if(updateStatus.TransferStatus == "Approved")
            {
                //Approve the transfer
                if(receiverAccount.Balance < transfer.Amount)
                {
                    int transferStatusId = transferStatusDao.GetTransferStatusByName("Rejected");
                    transfer.Transfer_Status_Id = transferStatusId;
                    transferDao.UpdateTransfer(transfer);
                    TxLog newLog = new TxLog()
                    {
                        Description = "Overdraft Attempted.",
                        Username_From = $"{User.Identity.Name}",
                        Username_To = $"{transferDao.GetAccountToUsername(transfer.Account_To)}",
                        Amount = transfer.Amount
                    };
                    TxLog.AddLog(newLog);
                }
                else
                {
                    senderAccount.Balance -= transfer.Amount;
                    receiverAccount.Balance += transfer.Amount;
                    if (transfer.Amount >= 1000)
                    {
                        //BasicLogger.Log($"Transfer by {User.Identity.Name}. Transfer Amount: {newTransferDto.Amount:C} Transfer To: {newTransferDto.Account_To}");
                        TxLog newLog = new TxLog()
                        {
                            Description = "Transfer made is greater than $1000",
                            Username_From = $"{User.Identity.Name}",
                            Username_To = $"{transferDao.GetAccountToUsername(transfer.Account_To)}",
                            Amount = transfer.Amount
                        };
                        TxLog.AddLog(newLog);
                    }
                    bool senderResult = accountDao.UpdateAccount(senderAccount.Account_Id, senderAccount.Balance);
                    bool receiverResult = accountDao.UpdateAccount(receiverAccount.Account_Id, receiverAccount.Balance);
                }
            }
            else if(updateStatus.TransferStatus == "Rejected")
            {
                //Reject the transfer
               // senderAccount.Balance += transfer.Amount;
               // bool senderResult = accountDao.UpdateAccount(senderAccount.Account_Id, senderAccount.Balance);
            }

            Transfer newTransfer = transferDao.GetTransferByTransferId(transfer.Transfer_Id);
            transfer = newTransfer;

            return Ok(transfer);
        }
    }
}
