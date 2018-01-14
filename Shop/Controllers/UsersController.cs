using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopModels;
using Shop.Helpers;
using Shop.Data;

namespace Shop.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private object _lock = new object();

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("UserExits/{id}")]
        public bool UserExists(string id)
        {
            if (!string.IsNullOrEmpty(id) && _dataContext.Users.Any(it => it.Name == id))
            {
                return true;
            }
            return false;
        }

        [HttpPost("RegisterUser")]
        public bool RegisterUser([FromBody]User user)
        {
            if(ModelState.IsValid )
            {
                lock (_lock)
                {
                    if (_dataContext.Users.Any(it => it.Name == user.Name))
                    {
                        // already registered user with such name
                        return false;
                    }
                    _dataContext.Users.Add(user);
                    _dataContext.Baskets.Add(new Basket() { UserToken = user.CalculateToken(), Products = new List<ProductCount>() });

                    _dataContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        [HttpPost("LogInUser")]
        public UserTokenResponse LogInUser([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                if (_dataContext.Users.Any(it => it.Name == user.Name && it.PasswordHash == user.PasswordHash))
                {
                    return new UserTokenResponse(true, user.CalculateToken());
                }
                return new UserTokenResponse(false);
            }
            return new UserTokenResponse(false);
        }
    }
}