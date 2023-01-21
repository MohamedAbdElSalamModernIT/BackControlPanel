
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Application.Drawings.Dto;
using System;

namespace Application.Drawings.Commands
{
    public class DeleteDrawingCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
    public class DeleteDrawingValidator : AbstractValidator<DeleteDrawingCommand>
    {
        public DeleteDrawingValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                           .WithMessage("id is Required");
        }
    }
    public class DeleteDrawingHndler : IRequestHandler<DeleteDrawingCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public DeleteDrawingHndler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }
        public async Task<Result> Handle(DeleteDrawingCommand request, CancellationToken cancellationToken)
        {
            var drawing = await _context.tblDrawings
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (drawing == null) return Result.Failure(ApiExceptionType.NotFound);

            _context.Remove(drawing);
            return Result.Successed(new { Id = request.Id }, ApiExceptionType.Ok);
        }
    }
}



