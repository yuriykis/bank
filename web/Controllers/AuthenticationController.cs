using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using web.Authorization.Models;
using web.Models;
using web.WebApi;

namespace web.Controllers
{
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRequests _userRequests;
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

        private bool _isUserAuthorized;

        public AuthenticationController(IHttpContextAccessor httpContextAccessor, IUserRequests userRequests)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRequests = userRequests;
        }
        
        private void SetCookie(string key, string value, int? expireTime)  
        {  
            var option = new CookieOptions();  
            if (expireTime.HasValue)  
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);  
            else  
                option.Expires = DateTime.Now.AddMinutes(10);  
            Response.Cookies.Append(key, value, option);  
        } 
        private string GetCookie(string key)  
        {  
            return Request.Cookies[key];  
        }
        private void RemoveCookie(string key)  
        {  
            Response.Cookies.Delete(key);  
        } 
        
        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            if (_isUserAuthorized)
            {
                return RedirectToAction("", "Home", new { area = "" });
            }
            
            return View();
        }
        
        
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var authenticateResponse = await _userRequests.LoginUser(new AuthenticateRequest
            {
                Username = username,
                Password = password
            });
            
            if (authenticateResponse.Id == null || authenticateResponse.Token == null)
            {
                TempData["message"] = "Data is incorrect. Please try again";
                return RedirectToAction("login", "Authentication");
            }

            SetCookie("UserId", authenticateResponse.Id, 10);
            SetCookie("Token", authenticateResponse.Token, 100);

            return RedirectToAction("", "Home", new { area = "" });
        }
        
        public IActionResult Logout()
        {
            RemoveCookie("UserId");
            RemoveCookie("Token");
            return RedirectToAction("login", "Authentication");
        }
        
    }
}