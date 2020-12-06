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

        
        [HttpGet]
        [Route("out_transaction_list")]
        public async Task<IActionResult> ViewOutgoingTransactionsList()
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            var profile = new TransactionProfile();
            var token = GetCookie("Token");
            var userId = GetCookie("UserId");
            var account = await _accountRequests.GetUserAccountData(token, userId);
                
            var transactionModelList = await _transactionRequests.GetTransactionBySenderList(token, account.Id);
            
            if (transactionModelList == null)
            {
                TempData["message"] = "Could not get user data. User service is probably unavailable";
            }
            else
            {
                foreach (var transactionModel in transactionModelList)
                {
                    profile.TransactionModelList.Add(new TransactionModel
                    {
                        ReceiverAccountId = transactionModel.ReceiverAccountId,
                        Amount = transactionModel.Amount,
                        Status = transactionModel.Status,
                        Info = transactionModel.Info
                    });
                }
            }

            return View(profile);
        }
        
        [HttpGet]
        [Route("in_transaction_list")]
        public async Task<IActionResult> ViewIncomingTransactionsList()
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            var profile = new TransactionProfile();
            var token = GetCookie("Token");
            var userId = GetCookie("UserId");
            var account = await _accountRequests.GetUserAccountData(token, userId);
                
            var transactionModelList = await _transactionRequests.GetTransactionByReceiverList(token, account.Id);
            
            if (transactionModelList == null)
            {
                TempData["message"] = "Could not get user data. User service is probably unavailable";
            }
            else
            {
                foreach (var transactionModel in transactionModelList)
                {
                    profile.TransactionModelList.Add(new TransactionModel
                    {
                        SenderAccountId = transactionModel.SenderAccountId,
                        Amount = transactionModel.Amount,
                        Status = transactionModel.Status,
                        Info = transactionModel.Info
                    });
                }
            }

            return View(profile);
        }
    }
}