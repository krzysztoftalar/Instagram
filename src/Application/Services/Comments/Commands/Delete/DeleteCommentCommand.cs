using System;
using MediatR;

namespace Application.Services.Comments.Commands.Delete
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}