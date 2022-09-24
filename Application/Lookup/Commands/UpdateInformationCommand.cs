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
using Application.Lookup.Dtos;

namespace Application.Lookup.Commands
{
    public class UpdateInformationCommand : IRequest<Result>
    {
        public double Id { get; set; }
        public string Description { get; set; }
  
    }

    public class UpdateInformationCommandValidator : AbstractValidator<UpdateInformationCommand>
    {
        public UpdateInformationCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull()
                .WithMessage("Id is Required");

            RuleFor(r => r.Description).NotEmpty().NotNull()
               .WithMessage("Name is Required");
       
        }
    }

    public class UpdateInformationCommandHandler : IRequestHandler<UpdateInformationCommand, Result>
    {
        private readonly IAppDbContext _context;
       

        public UpdateInformationCommandHandler(IAppDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
        {
            var information = await _context.tblInformation.FirstOrDefaultAsync(e => e.ID == request.Id);

            if (information == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedInformation = request.Adapt(information);

            _context.Edit(updatedInformation);

            return Result.Successed(updatedInformation.Adapt<InformationDto>());
        }
    }
}
