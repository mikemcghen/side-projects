using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public interface ITransferStatusDao
    {
        bool UpdateTransferStatus(int transferId, string transferStatus);
        int GetTransferStatusByName(string transferStatusName);
    }
}
