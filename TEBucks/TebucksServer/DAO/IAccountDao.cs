using System.Collections.Generic;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface IAccountDao
    {
        Account GetBalance(int id);
        List<Transfer> GetListOfTransfer(int id);
        List<Transfer> GetTransfersByAccountId(string username);
        Account GetAccountById(int accountId);
        public bool UpdateAccount(int accountid, decimal newBalance);
    }
}
