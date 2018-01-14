using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
    internal class ApiClient
    {
        private static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async Task<T> ProcessGetApiRequest<T>(string endPoint, Func<string, T> success, Func<T> fail)
        {
            using (var client = GetClient())
            {
                var result = await client.GetAsync(endPoint);

                if (!result.IsSuccessStatusCode)
                {
                    return fail.Invoke();
                }

                var content = await result.Content.ReadAsStringAsync();
                return success.Invoke(content);
            }
        }

        public static async Task<T> ProcessPostApiRequest<T>(string endPoint, Func<string, T> success, Func<T> fail, string jsonInput)
        {
            using (var client = GetClient())
            {
                var postContent = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(endPoint, postContent);

                if (!result.IsSuccessStatusCode)
                {
                    return fail.Invoke();
                }

                var content = await result.Content.ReadAsStringAsync();
                return success.Invoke(content);
            }
        }

        public static bool StringToBool(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return false;
            }

            if(bool.TryParse(input, out bool result))
            {
                return result;
            }
            return false;
        }
    }
}
