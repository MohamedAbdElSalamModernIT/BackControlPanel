using Application.Baladya.Dto;
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

namespace Application.Baladya.Commands
{
    public class CreateBaladyaCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public int AmanaId { get; set; }
    }
    public class CreateBaladyaValidator : AbstractValidator<CreateBaladyaCommand>
    {
        public CreateBaladyaValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");
            
            RuleFor(r => r.AmanaId).NotEmpty()
                  .WithMessage("AmanaId is Required");
        }
    }
    public class CreateBaladyaHandler : IRequestHandler<CreateBaladyaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public CreateBaladyaHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateBaladyaCommand request, CancellationToken cancellationToken)
        {
            var amana =await _context._tblAlamanat.FirstOrDefaultAsync(e=>e.ID==request.AmanaId);
            if (amana == null) return Result.Failure(ApiExceptionType.BadRequest);
            var baladya = request.Adapt<Domain.Entities.Benaa.Baladia>();

            var maxId = await _context.tblAlBaladiat.MaxAsync(e => e.ID);
            baladya.ID = maxId + 1;

            await _context.CreateAsync(baladya, cancellationToken);
            baladya.Amana = amana;
            return Result.Successed(baladya.Adapt<BaladyaDto>());
        }
    }
}
