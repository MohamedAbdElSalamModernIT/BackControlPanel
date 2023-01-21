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
    public class EditDrawingStatusCommand : IRequest<Result>, IRegister
    {
        public string Id { get; set; }
        public string Comments { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public OfficeDrawingStatus OfficeStatus { get; set; } = OfficeDrawingStatus.InProgress;

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EditDrawingStatusCommand, Drawing>()
                .Ignore(dest => dest.Extension);
        }
    }

    public class EditDrawingStatusCommandValidator : AbstractValidator<EditDrawingStatusCommand>
    {
        public EditDrawingStatusCommandValidator()
        {
            RuleFor(r => r.Id).NotNull()
                  .WithMessage("Id is Required");
            
            RuleFor(r => r.PlannedEndDate).NotNull().NotEmpty()
                  .WithMessage("PlannedEndDate is Required");
            
            RuleFor(r => r.ActualStartDate).NotNull().NotEmpty()
                  .WithMessage("ActualStartDate is Required");

        }
    }
    public class EditDrawingStatusHandler : IRequestHandler<EditDrawingStatusCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public EditDrawingStatusHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(EditDrawingStatusCommand request, CancellationToken cancellationToken)
        {
            var drwaing = await _context.tblDrawings
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (drwaing == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedDrwaing = request.Adapt(drwaing);

            _context.Edit(updatedDrwaing);
            return Result.Successed(drwaing.Adapt<DrwaingPluginDto>());
        }
    }

}
