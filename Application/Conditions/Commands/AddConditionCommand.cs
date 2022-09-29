using Application.Conditions.Dtos;
using Common;
using Common.Infrastructures;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using System.Collections.Generic;
using Domain.Entities.Benaa;
using System.Linq;

namespace Application.Conditions.Commands
{
    public class AddConditionCommand : IRequest<Result>
    {
        public int BuildingTypeID { get; set; }
        public int AlBaladiaID { get; set; }
        public double ConditionID { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class AddConditionCommandValidator : AbstractValidator<AddConditionCommand>
    {
        public AddConditionCommandValidator()
        {
            RuleFor(r => r.BuildingTypeID).NotEmpty().NotNull()
                .WithMessage("BuildingTypeId is Required");

            RuleFor(r => r.AlBaladiaID).NotEmpty().NotNull()
               .WithMessage("BaladyaId is Required");

            RuleFor(r => r.ConditionID).NotEmpty().NotNull()
               .WithMessage("ConditionId is Required");

        }
    }

    public class AddConditionCommandHandler : IRequestHandler<AddConditionCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public AddConditionCommandHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(AddConditionCommand request, CancellationToken cancellationToken)
        {
            var condition = request.Adapt<ConditionsMap>();
            var conditionmap = await _context.tblConditionsMap
                .Where(e => e.AlBaladiaID == request.AlBaladiaID
                && e.ConditionID == request.ConditionID
                && e.BuildingTypeID == request.BuildingTypeID
                ).FirstOrDefaultAsync(cancellationToken);

            var referenceXml = await _context.tblConditionsMap
               .Where(e => e.AlBaladiaID == 1
                && e.ConditionID == request.ConditionID
                ).FirstOrDefaultAsync(cancellationToken);

            var values = xmlService.UpdateXml(request.Parameters, referenceXml.ParametersValues);
            condition.ParametersValues = values;
            if (conditionmap != null)
            {
                conditionmap.ParametersValues = values;
                _context.Edit(conditionmap);
            }
            else
            {
                await _context.CreateAsync(condition);
            }
            return Result.Successed();
        }
    }
}
