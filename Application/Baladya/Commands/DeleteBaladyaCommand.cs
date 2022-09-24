
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Application.Baladya.Dto;

namespace Application.Baladya.Commands
{
    public class DeleteBaladyaCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteBaladyaValidator : AbstractValidator<DeleteBaladyaCommand>
    {
        public DeleteBaladyaValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                           .WithMessage("id is Required");
        }
    }
    public class DeleteBaladyaHndler : IRequestHandler<DeleteBaladyaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public DeleteBaladyaHndler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteBaladyaCommand request, CancellationToken cancellationToken)
        {
            var baladya = await _context.tblAlBaladiat
                .FirstOrDefaultAsync(e => e.ID == request.Id, cancellationToken);

            if (baladya == null) return Result.Failure(ApiExceptionType.NotFound);
            _context.tblAlBaladiat.Remove(baladya);

            var baladyaDto = baladya.Adapt<BaladyaDto>();
            return Result.Successed(baladyaDto, ApiExceptionType.Ok);
        }
    }
}
