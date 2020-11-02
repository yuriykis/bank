using api.Models;
using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace transactionsApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public ActionResult<List<Transaction>> Get() =>
            _transactionService.Get();


        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Transaction transactionIn)
        {
            var transaction = _transactionService.Get(id);

            if (transaction == null)
            {
                return NotFound();
            }

            _transactionService.Update(id, transactionIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var transaction = _transactionService.Get(id);

            if (transaction == null)
            {
                return NotFound();
            }

            _transactionService.Remove(transaction.id);

            return NoContent();
        }
    }
}