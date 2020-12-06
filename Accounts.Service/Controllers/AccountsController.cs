using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Service.Authorization.Helpers;
using Accounts.Service.Commands;
using Accounts.Service.Models;
using Accounts.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Accounts.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Account>>> Get()
        {
            var query = new GetAllAccountsQuery();
            try
            {
                var result = await _mediator.Send(query);
                _logger.LogInformation("List all accounts information request");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Error while processing List all accounts information request:" + e);
                return NotFound();
            }
        }
        
        [HttpGet(template: "{id}", Name = "GetAccount")]
        [Authorize]
        public async Task<ActionResult<Account>> Get(String id)
        {
            try
            {
                var query = new GetAccountByIdQuery(id);
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get account by id " + id + " request");
                return result != null ? (ActionResult<Account>) Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while processing Get account by id " + id + " request: " + e);
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("user/{id}")]
        public async Task<ActionResult<Account>> GetByUserId(string id)
        {
            try
            {
                var query = new GetAccountByUserIdQuery(id);
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get account by user id " + id + " request");
                return result != null ? (ActionResult<Account>) Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error while account creating");
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<Account> Create(CreateAccountCommand command)
        {
            try
            {
                var account = await _mediator.Send(command);
                _logger.LogInformation("Account has been successfully created");
                return account;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error while account creating: " + e);
                return new Account();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody]Account accountIn)
        {
            var command = new UpdateAccountCommand(id, accountIn);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("Account " + id + " was updated");
                return NoContent();
            }
            _logger.LogInformation("Error while account updating");
            return NotFound();
        }

        [HttpDelete("{id:length(50)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteAccountCommand(id);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("Account " + id + " was deleted");
                return NoContent();
            }

            return NotFound();
        }
    }
}