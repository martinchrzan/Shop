using System.Threading.Tasks;
using ShopModels;
using System;
using Newtonsoft.Json;

namespace ClientLib
{
    public class UserApi
    {
        private const string ApiPrefix = "/Users/";
        private readonly string _endPointAddress;
        
        public UserApi(string endPointAddress)
        {
            _endPointAddress = endPointAddress;
        }

        public async Task<bool> UserExists(string userName)
        {
            Func<string, bool> success = (result) => { return ApiClient.StringToBool(result); };

            Func<bool> fail = () => false;

            return await ApiClient.ProcessGetApiRequest(_endPointAddress + ApiPrefix + "UserExits/" + userName, success, fail);
        }

        public async Task<UserTokenResponse> LogInUser(string name, string passwordHash)
        {
            var postContent = GetUserJson(name, passwordHash);

            Func < string, UserTokenResponse> success = (result) => {
                return JsonConvert.DeserializeObject<UserTokenResponse>(result);
            };

            Func<UserTokenResponse> fail = () => new UserTokenResponse(false);

            return await ApiClient.ProcessPostApiRequest(_endPointAddress + ApiPrefix + "LogInUser", success, fail, postContent);
        }

        public async Task<bool> RegisterUser(string name, string passwordHash)
        {
            var postContent = GetUserJson(name, passwordHash);

            Func<string, bool> success = (result) => { return ApiClient.StringToBool(result); };

            Func<bool> fail = () => false;

            return await ApiClient.ProcessPostApiRequest(_endPointAddress + ApiPrefix + "RegisterUser", success, fail, postContent);
        }

        private static string GetUserJson(string name, string passwordHash)
        {
            var user = new User() { Name = name, PasswordHash = passwordHash };
            return JsonConvert.SerializeObject(user);
        }
    }
}
