using Application.Drawings.Dto;
using Common;
using Common.Exceptions;
using Domain.Entities.Benaa;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Drawings.Commands
{
    public class FinishDrawingCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public DrawingStatus Status{ get; set; }
      
    }

    public class FinishDrawingCommandValidator : AbstractValidator<FinishDrawingCommand>
    {
        public FinishDrawingCommandValidator()
        {
            RuleFor(r => r.Id).NotNull()
                  .WithMessage("Id is Required");
            RuleFor(r => r.Status).NotNull()
                  .WithMessage("Status is Required");

        }
    }
    public class FinishDrawingHandler : IRequestHandler<FinishDrawingCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public FinishDrawingHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(FinishDrawingCommand request, CancellationToken cancellationToken)
        {
            var drwaing = await _context.tblDrawings
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (drwaing == null) return Result.Failure(ApiExceptionType.NotFound);

            drwaing.Status = request.Status;
            drwaing.UpdatedDate = DateTime.UtcNow;
            drwaing.UpdatedBy = auditService.UserName;

            _context.tblDrawings.Update(drwaing);
            return Result.Successed(drwaing.Adapt<DrwaingPluginDto>());
        }
    }

}
