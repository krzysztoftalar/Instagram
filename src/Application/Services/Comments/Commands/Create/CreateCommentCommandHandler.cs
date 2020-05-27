using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;

namespace Application.Services.Comments.Commands.Create
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
              .Include(x => x.Photos)
              .SingleOrDefaultAsync(x =>
                  x.UserName == request.Username, cancellationToken: cancellationToken);

            var comment = new Comment
            {
                PhotoId = request.PhotoId,
                Author = user,
                Body = request.Body,
                CreatedAt = DateTime.Now
            };

            await _context.Comments.AddAsync(comment, cancellationToken);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return _mapper.Map<CommentDto>(comment);

            throw new Exception("Problem saving changes");
        }
    }
}
