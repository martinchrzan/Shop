using Newtonsoft.Json;
using ShopModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientLib
{
    public class ShopApi
    {
        private readonly string _endPointAddress;
        private const string ApiPrefix = "/Shop/";

        public ShopApi(string endPointAddress)
        {
            _endPointAddress = endPointAddress;
        }

        public async Task<List<Product>> ListProducts()
        {
            Func<string, List<Product>> success = (result) => {
                return JsonConvert.DeserializeObject<List<Product>>(result);
            };

            Func<List<Product>> fail = () => new List<Product>();

            return await ApiClient.ProcessGetApiRequest(_endPointAddress + ApiPrefix + "ListProducts", success, fail);
        }

        public async Task<Basket> GetCustomerProducts(string userToken)
        {
            Func<string, Basket> success = (result) => {
                return JsonConvert.DeserializeObject<Basket>(result);
            };

            Func<Basket> fail = () => new Basket();

            return await ApiClient.ProcessGetApiRequest(_endPointAddress + ApiPrefix + "GetCustomerProducts/" + userToken, success, fail);
        }

        public async Task<bool> AddCustomerProduct(string userToken, int productId, int count)
        {
            return await ProcessProductItemRequest(userToken, productId, count, "AddCustomerProduct");
        }

        public async Task<bool> RemoveCustomerProduct(string userToken, int productId, int count)
        {
            return await ProcessProductItemRequest(userToken, productId, count, "RemoveCustomerProduct");
        }

        public async Task<bool> ClearProducts(string userToken)
        {
            return await ProcessUserTokenRequest(userToken, "ClearProducts/");
        }

        public async Task<bool> PurchaseProducts(string userToken)
        {
            return await ProcessUserTokenRequest(userToken, "PurchaseProducts/");
        }

        private async Task<bool> ProcessProductItemRequest(string userToken, int productId, int count, string endPointSuffix)
        {
            var postContent = GetBasketItemJson(userToken, productId, count);

            Func<string, bool> success = (result) => { return ApiClient.StringToBool(result); };

            Func<bool> fail = () => false;

            return await ApiClient.ProcessPostApiRequest(_endPointAddress + ApiPrefix + endPointSuffix, success, fail, postContent);
        }

        private async Task<bool> ProcessUserTokenRequest(string userToken, string endPointSuffix)
        {
            Func<string, bool> success = (result) => { return ApiClient.StringToBool(result); };

            Func<bool> fail = () => false;

            return await ApiClient.ProcessPostApiRequest(_endPointAddress + ApiPrefix + endPointSuffix + userToken, success, fail, userToken);
        }

        private string GetBasketItemJson(string userToken, int productId, int count)
        {
            var basketItem = new BasketItem()
            {
                UserToken = userToken,
                ProductCount = new ProductCount() { ProductId = productId, Count = count }
            };
            return JsonConvert.SerializeObject(basketItem);
        }
    }
}
