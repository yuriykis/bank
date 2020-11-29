using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Service.Authorization.Helpers;
using Accounts.Service.Commands;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Account>>> Get()
        {
            var query = new GetAllAccountsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet(template: "{id}", Name = "GetAccount")]
        [Authorize]
        public async Task<ActionResult<Account>> Get(String id)
        {
            var query = new GetAccountByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (ActionResult<Account>) Ok(result) : NotFound();
        }
        
        

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, Account accountIn)
        {
            var command = new UpdateAccountCommand(id, accountIn);
            var result = await _mediator.Send(command);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteAccountCommand(id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}