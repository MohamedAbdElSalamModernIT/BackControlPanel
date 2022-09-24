using Common;
using Domain.Entities.Benaa;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Conditions.Commands
{
    public class DeleteConditionCommand : IRequest<Result>
    {
        public int BuildingTypeID { get; set; }
        public int AlBaladiaID { get; set; }
        public double ConditionID { get; set; }
    }
    public class DeleteConditionCommandValidator : AbstractValidator<DeleteConditionCommand>
    {
        public DeleteConditionCommandValidator()
        {
            RuleFor(r => r.BuildingTypeID).NotEmpty().NotNull()
                .WithMessage("BuildingTypeId is Required");

            RuleFor(r => r.AlBaladiaID).NotEmpty().NotNull()
               .WithMessage("BaladyaId is Required");

            RuleFor(r => r.ConditionID).NotEmpty().NotNull()
               .WithMessage("ConditionId is Required");

        }
    }

    public class DeleteConditionCommandHandler : IRequestHandler<DeleteConditionCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public DeleteConditionCommandHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(DeleteConditionCommand request, CancellationToken cancellationToken)
        {
            var condition = request.Adapt<ConditionsMap>();
            var conditionmap = await _context.tblConditionsMap
                .Where(e => e.AlBaladiaID == request.AlBaladiaID
                && e.ConditionID == request.ConditionID
                && e.BuildingTypeID == request.BuildingTypeID
                ).FirstOrDefaultAsync(cancellationToken);


            if (conditionmap == null)
            {
                return Result.Failure(Common.Exceptions.ApiExceptionType.NotFound);
            }
                _context.tblConditionsMap.Remove(conditionmap);

            return Result.Successed(conditionmap);
        }
    }
}
