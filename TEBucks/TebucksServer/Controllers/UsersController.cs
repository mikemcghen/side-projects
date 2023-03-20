using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TEbucksServer.Models;
using TEBucksServer.DAO;
using TEBucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUserDao userDao;

        public UsersController(IUserDao _userDao)
        {
            this.userDao = _userDao;
        }
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(userDao.GetAllUsers());
        }
    }
}
