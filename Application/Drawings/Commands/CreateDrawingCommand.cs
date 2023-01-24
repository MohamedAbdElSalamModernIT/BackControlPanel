using Application.Drawings.Dto;
using Common;
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
    public class CreateDrawingCommand : IRequest<Result>
    {
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public OfficeDrawingStatus OfficeStatus { get; set; } = OfficeDrawingStatus.OnHold;
        public int BuildingTypeId { get; set; }
        public int BaladiaId { get; set; }
        public string CustomerName { get; set; }
        //public string FileStr { get; set; }
        //public string Extension { get; set; }

    }

    public class CreateDrawingValidator : AbstractValidator<CreateDrawingCommand>
    {
        public CreateDrawingValidator()
        {
            RuleFor(r => r.FileType).NotNull()
                  .WithMessage("FileType is Required");

            RuleFor(r => r.DrawingType).NotNull()
                  .WithMessage("DrawingType is Required");

            RuleFor(r => r.BaladiaId).NotNull()
                  .WithMessage("Baladya is Required");

            RuleFor(r => r.BuildingTypeId).NotNull()
                  .WithMessage("BuildingType is Required");
        }
    }
    public class CreateDrawingHandler : IRequestHandler<CreateDrawingCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public CreateDrawingHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(CreateDrawingCommand request, CancellationToken cancellationToken)
        {
            var drwaing = request.Adapt<Drawing>();

            drwaing.OfficeId = auditService.OfficeId;
            drwaing.EngineerId = auditService.UserId;

            //if (!string.IsNullOrEmpty(request.FileStr))
            //{
            //    var bytes = Convert.FromBase64String(request.FileStr);
            //    drwaing.File = bytes;
            //    drwaing.Extension = request.Extension;
            //}

            await _context.CreateAsync(drwaing, cancellationToken);
            return Result.Successed(drwaing.Adapt<DrwaingPluginDto>());
        }
    }

}
