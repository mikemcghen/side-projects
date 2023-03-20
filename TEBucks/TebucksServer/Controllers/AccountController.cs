using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TEbucksServer.DAO;
using TEbucksServer.Models;
using TEBucksServer.DAO;
using TEBucksServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace TEbucksServer.Controllers
{
    [Route("api/account/")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountDao accountDao;
        private IUserDao userDao;

        public AccountController(IAccountDao _accountDao, IUserDao userDao)
        {
            this.accountDao = _accountDao;
            this.userDao = userDao;

        }
        [HttpGet("balance")]
        public ActionResult<Account> GetAccountBalance()
        {
            string username = User.Identity.Name;
            int userId = userDao.GetUser(username).UserId;
            Account account = accountDao.GetBalance(userId);
            if(account != null)
            {
                return Ok(account); 
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("transfers")]
        public ActionResult<List<Transfer>> GetListOfTransfer()
        {
                string username = User.Identity.Name;
                int userId = userDao.GetUser(username).UserId;
                return Ok(accountDao.GetListOfTransfer(userId));
     

        }
    }
}
