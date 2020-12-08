using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using web.Authorization.Models;
using web.Models;

namespace web.WebApi
{
    public class UserRequests : IUserRequests
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private const string UserServiceHost = "http://nginx:80/";
        
        public async Task<UserModel> GetUserData(string token, string userId)
        {
            var path = "api/users/" + userId;
            var requestUrl = UserServiceHost + path;
            
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",token);
                
                var responseMessage = await _httpClient.GetAsync(requestUrl);
                var content = responseMessage.Content;
                var response = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(response);
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

        public async Task<AuthenticateResponse> LoginUser(AuthenticateRequest authenticateRequest)
        {
            const string path = "api/users/authenticate";
            const string requestUrl = UserServiceHost + path;
            
            try
            {
                var postContent = new StringContent(JsonConvert.SerializeObject(authenticateRequest), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync(requestUrl, postContent);
                
                var content = responseMessage.Content;
                var response = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthenticateResponse>(response);
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

        public async Task<bool> RegisterUser(UserModel userModel)
        {
            const string path = "api/users/";
            const string requestUrl = UserServiceHost + path;

            try
            {
                var postContent = new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync(requestUrl, postContent);
                
                var status = responseMessage.StatusCode;
                return status == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}