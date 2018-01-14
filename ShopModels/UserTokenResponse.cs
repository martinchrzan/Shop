using System;
using System.Collections.Generic;
using System.Text;

namespace ShopModels
{
    public class UserTokenResponse
    {
        public UserTokenResponse()
        {
        }

        public UserTokenResponse(bool isValid)
        {
            IsValid = isValid;
        }

        public UserTokenResponse(bool isValid, string userToken) : this(isValid)
        {
            UserToken = userToken;
        }

        public string UserToken { get; set; }

        public bool IsValid { get; set; }
    }
}
