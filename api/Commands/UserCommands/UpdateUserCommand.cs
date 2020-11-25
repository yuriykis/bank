using System;
using api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public String Id { get; set; }
        public User user { get; set; }

        public UpdateUserCommand(string id, User user)
        {
            Id = id;
            this.user = user;
        }
    }
}