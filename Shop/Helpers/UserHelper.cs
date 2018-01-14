using ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Helpers
{
    public static class UserHelper
    {
        public static string CalculateToken(this User user)
        {
            if(user != null && !string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.PasswordHash))
            {
                // just some dummy calculation for now
                return user.Name + user.PasswordHash;
            }
            return string.Empty;
        }
    }
}
