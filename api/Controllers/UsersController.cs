using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Commands;
using api.Queries;
using MediatR;

namespace UsersApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet(template: "{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(String id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (ActionResult<User>) Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
            
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(String id, [FromBody] User userIn)
        {   var command = new UpdateUserCommand(id, userIn);
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
            var command = new DeleteUserCommand(id);
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