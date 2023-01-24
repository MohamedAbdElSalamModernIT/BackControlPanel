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
    public class EditDrawingCommand : IRequest<Result>, IRegister
    {
        public string Id { get; set; }
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        public int BaladiaId { get; set; }
        public string EngineerId { get; set; }
        public int BuildingTypeId { get; set; }
        public string CustomerName { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public OfficeDrawingStatus OfficeStatus { get; set; } = OfficeDrawingStatus.OnHold;

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EditDrawingCommand, Drawing>()
                .Ignore(dest => dest.Extension);
        }
    }

    public class EditDrawingCommandValidator : AbstractValidator<EditDrawingCommand>
    {
        public EditDrawingCommandValidator()
        {
            RuleFor(r => r.Id).NotNull()
                  .WithMessage("Id is Required");

            RuleFor(r => r.FileType).NotNull()
                  .WithMessage("FileType is Required");

            RuleFor(r => r.PlannedStartDate).NotNull().NotEmpty()
                  .WithMessage("PlannedStartDate is Required");

            RuleFor(r => r.CustomerName).NotNull()
                  .WithMessage("CustomerName is Required");

            RuleFor(r => r.DrawingType).NotNull()
                  .WithMessage("DrawingType is Required");

            RuleFor(r => r.BaladiaId).NotNull()
                  .WithMessage("Baladya is Required");

            RuleFor(r => r.BuildingTypeId).NotNull()
                  .WithMessage("BuildingType is Required");
        }
    }
    public class EditDrawingHandler : IRequestHandler<EditDrawingCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public EditDrawingHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(EditDrawingCommand request, CancellationToken cancellationToken)
        {
            var drwaing = await _context.tblDrawings
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (drwaing == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedDrwaing = request.Adapt(drwaing);

            if (string.IsNullOrEmpty(request.EngineerId))
            {
                drwaing.EngineerId = null;
                drwaing.OfficeStatus = OfficeDrawingStatus.NotAssigned;
            }

            _context.Edit(updatedDrwaing);
            return Result.Successed(drwaing.Adapt<DrwaingPluginDto>());
        }
    }

}
