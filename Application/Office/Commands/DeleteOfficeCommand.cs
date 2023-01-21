using Application.Office.Dtos;

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

namespace Application.Office.Commands
{
    public class DeleteOfficeCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
    public class DeleteOfficeValidator : AbstractValidator<DeleteOfficeCommand>
    {
        public DeleteOfficeValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                           .WithMessage("id is Required");
        }
    }
    public class DeleteOfficeHndler : IRequestHandler<DeleteOfficeCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteOfficeHndler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var Office = await _context.tblOffices
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (Office == null) return Result.Failure(ApiExceptionType.NotFound);
            _context.Remove(Office);

            var OfficeDto = Office.Adapt<OfficeDto>();
            return Result.Successed(OfficeDto, ApiExceptionType.Ok);
        }
    }
}
