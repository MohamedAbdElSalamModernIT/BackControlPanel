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
        public int BuildingTypeId { get; set; }
        public string CustomerName { get; set; }
        public string FileStr { get; set; }
        public string Extension { get; set; }

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

        

            if (!string.IsNullOrEmpty(request.FileStr))
            {
                var bytes = Convert.FromBase64String(request.FileStr);
                updatedDrwaing.File = bytes;
                updatedDrwaing.Extension = request.Extension;
            }

             _context.tblDrawings.Update(updatedDrwaing);
            return Result.Successed(drwaing.Adapt<DrwaingPluginDto>());
        }
    }

}
