using System.Collections.Generic;
using TEBucksServer.Models;

namespace TEBucksServer.DAO
{
    public interface IUserDao
    {
        User GetUser(string username);
        User AddUser(string username, string password);
        List<User> GetUsers();
        List<User> GetAllUsers();
    }
}
