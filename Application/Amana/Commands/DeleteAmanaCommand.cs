
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Application.Amana.Dto;


namespace Application.Amana.Commands
{
    public class DeleteAmanaCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteAmanaValidator : AbstractValidator<DeleteAmanaCommand>
    {
        public DeleteAmanaValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                           .WithMessage("id is Required");
        }
    }
    public class DeleteAmanaHndler : IRequestHandler<DeleteAmanaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteAmanaHndler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteAmanaCommand request, CancellationToken cancellationToken)
        {
            var amana = await _context._tblAlamanat
                .FirstOrDefaultAsync(e => e.ID == request.Id, cancellationToken);

            if (amana == null) return Result.Failure(ApiExceptionType.NotFound);
            _context._tblAlamanat.Remove(amana);

            var amanaDto = amana.Adapt<AmanaDto>();
            return Result.Successed(amanaDto, ApiExceptionType.Ok);
        }
    }
}
