using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transactions.Service.Authorization.Helpers;
using Transactions.Service.Authorization.Models;
using Transactions.Service.Commands;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;
using Microsoft.Extensions.Logging;

namespace Transactions.Service.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionsController> _logger;
        private new HttpContext HttpContext { get; }

        public TransactionsController(IMediator mediator, IHttpContextAccessor contextAccessor, ILogger<TransactionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
            HttpContext = contextAccessor.HttpContext;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Transaction>> Create([FromBody] CreateTransactionCommand command)
        {
            try
            {
                var authenticationUser = (AuthenticationUser)HttpContext.Items["auth"];
                command.UserId = authenticationUser.UserId;
                var result = await _mediator.Send(command);
                _logger.LogInformation("New transaction from " + command.SenderAccountId + " to " + command.ReceiverAccountId + " in the amount of " + command.Amount + " has been created");
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Transaction creation error");
                return NotFound();
            }
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Transaction>>> Get()
        {
            try
            {
                var query = new GetAllTransactionsQuery();
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get list of transactions request");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Error get list of transactions request: " + e);
                return NotFound();
            }
        }
        
        [HttpGet(template: "{id}", Name = "GetTransaction")]
        [Authorize]
        public async Task<ActionResult<Transaction>> Get(String id)
        {
            try
            {
                var query = new GetTransactionByIdQuery(id);
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get transaction by id" + id + " request");
                return result != null ? (ActionResult<Transaction>) Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Error Get transaction by id" + id + " request: " + e);
                return NotFound();
            }
        }

        [HttpGet(template: "account_sender/{accountId}")]
        [Authorize]
        public async Task<ActionResult<List<Transaction>>> GetByAccountSenderId(string accountId)
        {
            var query = new GetTransactionsByAccountSenderIdQuery(accountId);
            _logger.LogInformation("Get transactions by sender id " + accountId + " request");
            return await _mediator.Send(query);
        }
        
        [HttpGet(template: "account_receiver/{accountId}")]
        [Authorize]
        public async Task<ActionResult<List<Transaction>>> GetByAccountReceiverId(string accountId)
        {
            var query = new GetTransactionsByAccountReceiverIdQuery(accountId);
            _logger.LogInformation("Get transactions by sender id " + accountId + " request");
            return await _mediator.Send(query);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(String id, [FromBody] Transaction transactionIn)
        {
            var command = new UpdateTransactionCommand(id, transactionIn);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("Update transaction " + id + " request");
                return NoContent();
            }

            _logger.LogError("Error Update transaction " + id + " request: ");
            return NotFound();
        }
        

        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteTransactionCommand(id);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("Delete transaction " + id + " request");
                return NoContent();
            }

            _logger.LogError("Error Delete transaction " + id + " request: ");
            return NotFound();
        }
    }
}