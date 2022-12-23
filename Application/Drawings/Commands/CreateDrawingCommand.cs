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
        public int BaladiaId { get; set; }
        public int BuildingTypeId { get; set; }
        public string CustomerName { get; set; }

    }

    public class CreateDrawingValidator : AbstractValidator<CreateDrawingCommand>
    {
        public CreateDrawingValidator()
        {
            RuleFor(r => r.FileType).NotEmpty().NotNull()
                  .WithMessage("FileType is Required");

            RuleFor(r => r.DrawingType).NotEmpty().NotNull()
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
            drwaing.ClientId = auditService.UserId;

            await _context.CreateAsync(drwaing, cancellationToken);
            return Result.Successed(drwaing.Adapt<DrwaingDto>());
        }
    }

}
