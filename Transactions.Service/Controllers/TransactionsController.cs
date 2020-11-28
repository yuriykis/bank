﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Transactions.Service.Commands;
using Transactions.Service.Models;
using Transactions.Service.Queries.TransactionQueries;

namespace Transactions.Service.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> Get()
        {
            var query = new GetAllTransactionsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet(template: "{id}", Name = "GetTransaction")]
        public async Task<ActionResult<Transaction>> Get(String id)
        {
            var query = new GetTransactionByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (ActionResult<Transaction>) Ok(result) : NotFound();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(String id, [FromBody] Transaction transactionIn)
        {
            var command = new UpdateTransactionCommand(id, transactionIn);
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
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteTransactionCommand(id);
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