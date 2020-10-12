using api.Models;
using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace UsersApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService UserService)
        {
            _userService = UserService;
        }


        [HttpGet("{name}/{password}", Name = "GetUser")]
        public ActionResult<int> Get(String name, String password)
        {
            User user = _userService.Get().Find(w => w.name == name && w.password == password);
            if (user == null)
            {
                return 400;
            }
            else
            {
                return 200;
            }
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] Dictionary<String, String> userData)
        {
            string name = userData["name"];
            string password = userData["password"];
            User user = _userService.Get().Find(w => w.name == name);
            if (user == null)
            {
                User new_user = new User { name = name, password = password };
                _userService.Create(new_user);
                return 200;
            }
            else
            {
                return 400;
            }
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User UserIn)
        {
            var User = _userService.Get(id);

            if (User == null)
            {
                return NotFound();
            }

            _userService.Update(id, UserIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var User = _userService.Get(id);

            if (User == null)
            {
                return NotFound();
            }

            _userService.Remove(User.id);

            return NoContent();
        }
    }
}