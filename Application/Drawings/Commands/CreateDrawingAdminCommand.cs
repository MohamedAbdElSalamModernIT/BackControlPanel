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
    public class CreateDrawingAdminCommand : IRequest<Result>
    {
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public int BuildingTypeId { get; set; }
        public int BaladiaId { get; set; }
        public string CustomerName { get; set; }
        public string EngineerId { get; set; }
        //public string FileStr { get; set; }
        //public string Extension { get; set; }

    }

    public class CreateDrawingAdminValidator : AbstractValidator<CreateDrawingAdminCommand>
    {
        public CreateDrawingAdminValidator()
        {
            RuleFor(r => r.FileType).NotNull()
                  .WithMessage("FileType is Required");

            RuleFor(r => r.DrawingType).NotNull()
                  .WithMessage("DrawingType is Required");

            RuleFor(r => r.PlannedStartDate).NotNull().NotEmpty()
                 .WithMessage("PlannedStartDate is Required");

            RuleFor(r => r.CustomerName).NotNull()
                  .WithMessage("CustomerName is Required");

            RuleFor(r => r.BaladiaId).NotNull()
                  .WithMessage("Baladya is Required");

            RuleFor(r => r.BuildingTypeId).NotNull()
                  .WithMessage("BuildingType is Required");
        }
    }
    public class CreateDrawingAdminHandler : IRequestHandler<CreateDrawingAdminCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public CreateDrawingAdminHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(CreateDrawingAdminCommand request, CancellationToken cancellationToken)
        {
            var drwaing = request.Adapt<Drawing>();

            drwaing.OfficeId = auditService.OfficeId;
            if (string.IsNullOrEmpty(request.EngineerId))
            {
                drwaing.EngineerId = null;
                drwaing.OfficeStatus = OfficeDrawingStatus.NotAssigned;
            }
            else
            {
                drwaing.OfficeStatus = OfficeDrawingStatus.Assigned;
            }

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
