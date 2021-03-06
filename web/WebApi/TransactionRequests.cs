using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using web.Models;

namespace web.WebApi
{
    public class TransactionRequests : ITransactionRequests
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string TransactionServiceHost = "http://nginx:80/";

        public async Task<bool> CompleteTransaction(string token, TransactionModel transactionModel)
        {
            const string path = "api/transactions/";
            const string requestUrl = TransactionServiceHost + path;
            
            try
            {
                var postContent = new StringContent(JsonConvert.SerializeObject(transactionModel), Encoding.UTF8, "application/json");
                
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",token);
                
                var responseMessage = await _httpClient.PostAsync(requestUrl, postContent);
                
                var status = responseMessage.StatusCode;
                return status == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<TransactionModel>> GetTransactionBySenderList(string token, string accountId)
        {
            var path = "api/transactions/account_sender/";
            var requestUrl = TransactionServiceHost + path + accountId;
            
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",token);
                
                var responseMessage = await _httpClient.GetAsync(requestUrl);
                var content = responseMessage.Content;
                var response = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TransactionModel>>(response);
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

        public async Task<List<TransactionModel>> GetTransactionByReceiverList(string token, string accountId)
        {
            var path = "api/transactions/account_receiver/";
            var requestUrl = TransactionServiceHost + path + accountId;
            
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",token);
                
                var responseMessage = await _httpClient.GetAsync(requestUrl);
                var content = responseMessage.Content;
                var response = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TransactionModel>>(response);
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