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


namespace Application.Conditions.Commands
{
    public class UpdateConditionCommand : IRequest<Result>
    {
        public double Id { get; set; }
        public string Description { get; set; }
  
    }

    public class UpdateConditionValidator : AbstractValidator<UpdateConditionCommand>
    {
        public UpdateConditionValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull()
                .WithMessage("Id is Required");

            RuleFor(r => r.Description).NotEmpty().NotNull()
               .WithMessage("Name is Required");
       
        }
    }

    public class UpdateConditionHandler : IRequestHandler<UpdateConditionCommand, Result>
    {
        private readonly IAppDbContext _context;
       

        public UpdateConditionHandler(IAppDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(UpdateConditionCommand request, CancellationToken cancellationToken)
        {
            var condition = await _context.tblConditions.Include(e=>e.Version)
                .FirstOrDefaultAsync(e => e.ID == request.Id);

            if (condition == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedCondition = request.Adapt(condition);

            _context.Edit(updatedCondition);

            return Result.Successed(updatedCondition.Adapt<ConditionDto>());
        }
    }
}
