using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferByTransferId(int transfer);
        Transfer InsertTransfer(Transfer transfer);
        bool UpdateTransfer(Transfer transfer);
        string GetAccountToUsername(int accountId);
    }
}
