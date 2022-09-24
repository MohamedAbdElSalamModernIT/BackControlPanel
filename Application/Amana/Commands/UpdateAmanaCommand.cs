using Application.Amana.Dto;

using Common;
using Common.Infrastructures;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Benaa;

namespace Application.Amana.Commands
{
    public class UpdateAmanaCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateAmanaValidator : AbstractValidator<UpdateAmanaCommand>
    {
        public UpdateAmanaValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                 .WithMessage("Id is Required");
            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");
        }
    }
    public class UpdateAmanaHandler : IRequestHandler<UpdateAmanaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateAmanaHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateAmanaCommand request, CancellationToken cancellationToken)
        {
            var amana = await _context._tblAlamanat
                .FirstOrDefaultAsync(e => e.ID == request.Id, cancellationToken);

            if (amana == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedAmana = request.Adapt(amana);

            _context.Edit(updatedAmana);

            return Result.Successed(updatedAmana.Adapt<AmanaDto>());
        }
    }
}
