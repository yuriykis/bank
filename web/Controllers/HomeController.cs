using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using web.Domain.Entities;
using web.Models;
using web.WebApi;

namespace web.Controllers
{
    public class HomeController : Controller
    {
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
        
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRequests _userRequests;
        private readonly IAccountRequests _accountRequests;
        private bool _isUserAuthorized;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IUserRequests userRequests, IAccountRequests accountRequests)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userRequests = userRequests;
            _accountRequests = accountRequests;
        }
        
        private string GetCookie(string key)  
        {  
            return Request.Cookies[key];  
        }

        public IActionResult Index()
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            return RedirectToAction("AccountView");
        }

        [ActionName("privacy")]
        public IActionResult Privacy()
        {

            return View();
        }
        
        [Route("account")]
        public async Task<IActionResult> AccountView()
        {
            if (!_isUserAuthorized)
            {
                return RedirectToAction("login", "Authentication", new {area = ""});
            }
            
            var profile = new UserProfile();
            var token = GetCookie("Token");
            var userId = GetCookie("UserId");
            
            var userModel =  await _userRequests.GetUserData(token, userId);
            var accountModel = await _accountRequests.GetUserAccountData(token, userId);

            if (userModel == null)
            {
                TempData["message"] = "Could not get user data. User service is probably unavailable";
            }
            else
            {
                profile.UserData.Username = userModel.Username;
                profile.UserData.FirstName = userModel.FirstName;
                profile.UserData.LastName = userModel.LastName;
            }

            if (accountModel == null)
            {
                TempData["message"] = "Could not get account data. Account service is probably unavailable";
            }
            else
            {
                profile.UserData.Amount = Int32.Parse(accountModel.Amount);
                profile.UserData.AccountId = accountModel.Id;
            }

            return View(profile);
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            if (_isUserAuthorized)
            {
                return RedirectToAction("");
            }
            
            return View();
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string username, string firstName, string lastName, string password)
        {
            var userModel = new UserModel
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };
            
            var response =  await _userRequests.RegisterUser(userModel);
            if (response)
            {
                TempData["message"] = "The account has been successfully created";
                return RedirectToAction("");
            }
            TempData["message"] = "Account creation failed. Please try again later";
            return RedirectToAction("register");
        }
        
    }
}