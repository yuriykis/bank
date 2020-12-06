using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Users.Service.Authorization.Helpers;
using Users.Service.Authorization.Models;
using Users.Service.Commands;
using Users.Service.Models;
using Users.Service.Queries;

namespace Users.Service.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var query = new GetAllUsersQuery();
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get list of users request");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Error Get list of users request");
                return NotFound();
            }
        }
        
        [HttpGet(template: "{id}")]
        [Authorize]
        public async Task<ActionResult<User>> Get(String id)
        {
            try
            {
                var query = new GetUserByIdQuery(id);
                var result = await _mediator.Send(query);
                _logger.LogInformation("Get user by id " + id + " request");
                return result != null ? (ActionResult<User>) Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Error Get user by id " + id + " request");
                return NotFound();
            }
        }
        
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            try
            {
                var query = new GetUserByParamsQuery(request.Username, request.Password);
                
                var res = await _mediator.Send(query);
                if (res.Id == null)
                {
                    _logger.LogError("Authorization error. The login details provided are not correct");
                    return NotFound();
                }
                
                var command = new AuthenticateUserCommand
                {
                    Username = request.Username,
                    Password = request.Password
                };
                
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    _logger.LogError("Authorization error. Login failed");
                    return NotFound();
                }
                _logger.LogInformation("User " + request.Username + " has been logged in");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Authorization error. Login failed");
                return BadRequest(ex.Message);
            }
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                _logger.LogInformation("New user has been created");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating new user");
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(String id, [FromBody] User userIn)
        {   var command = new UpdateUserCommand(id, userIn);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("User " + id + "has been updated");
                return NoContent();
            }

            _logger.LogError("Error while updating user " + id);
            return NotFound();
        }
        
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);
            if (result)
            {
                _logger.LogInformation("User " + id + "has been deleted");
                return NoContent();
            }

            _logger.LogError("Error while deleting user " + id);
            return NotFound();
        }
        
    }
}