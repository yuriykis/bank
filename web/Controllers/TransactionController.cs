using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using web.Models;
using web.WebApi;

namespace web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRequests _accountRequests;
        private readonly ITransactionRequests _transactionRequests;
        private bool _isUserAuthorized;

        public TransactionController(IAccountRequests accountRequests, ILogger<HomeController> logger, ITransactionRequests transactionRequests)
        {
            _accountRequests = accountRequests;
            _logger = logger;
            _transactionRequests = transactionRequests;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (GetCookie("Token") == null || GetCookie("UserId") == null)
            {
                _isUserAuthorized = false;
            }
            else
            {
                _isUserAuthorized = true;
            }
        }
        
        private string GetCookie(string key)  
        {  
            return Request.Cookies[key];  
        }
        
        
        [HttpGet]
        [Route("transaction")]
        public IActionResult CompleteTransaction()
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            return View();
        }
        
        [HttpPost]
        [Route("transaction")]
        public async Task<IActionResult> CompleteTransaction(string receiverAccountId, int amount)
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            var userId = GetCookie("UserId");
            var token = GetCookie("Token");
            var accountServiceResponse = await _accountRequests.GetUserAccountData(token, userId);

            if (Int32.Parse(accountServiceResponse.Amount) < amount)
            {
                TempData["message"] = "Account balance too low";
                return RedirectToAction("CompleteTransaction");
            }
            
            var transactionModel = new TransactionModel
            {
                SenderAccountId = accountServiceResponse.Id,
                ReceiverAccountId = receiverAccountId,
                Amount = amount
            };
            
            var response =  await _transactionRequests.CompleteTransaction(token, transactionModel);
            if (response)
            {
                TempData["message"] = "The transaction was successful";
            }
            else
            {
                TempData["message"] = "Transaction failed";
            }
            return RedirectToAction("CompleteTransaction");
        }

    }
}