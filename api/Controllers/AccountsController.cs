using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace accountsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserService _userService;

        public AccountsController(AccountService accountService, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<Account>> Get() =>
            _accountService.Get();

        [HttpGet("{id:length(24)}", Name = "Getaccount")]
        public ActionResult<Account> Get(string id)
        {
            var account = _accountService.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }


        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Account accountIn)
        {
            var account = _accountService.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            _accountService.Update(id, accountIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var account = _accountService.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            _accountService.Remove(account.id);

            return NoContent();
        }
    }
}