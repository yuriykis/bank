using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using web.Models;

namespace web.WebApi
{
    public class AccountRequests : IAccountRequests
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string AccountsServiceHost = "http://nginx:80/";
        
        public async Task<AccountModel> GetUserAccountData(string token, string userId)
        {
            var path = "api/accounts/user/" + userId;
            var requestUrl = AccountsServiceHost + path;

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var responseMessage = await _httpClient.GetAsync(requestUrl);
                var content = responseMessage.Content;
                var response = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AccountModel>(response);
            }
            catch (HttpRequestException exception)
            {
                return null;
            }
            catch (JsonReaderException e)
            {
                return null;
            }
        }
    }
}