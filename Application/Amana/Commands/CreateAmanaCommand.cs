using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Benaa;
using Application.Amana.Dto;

namespace Application.Amana.Commands
{
    public class CreateAmanaCommand : IRequest<Result>
    {
        public string Name { get; set; }
    }
    public class CreateAmanaValidator : AbstractValidator<CreateAmanaCommand>
    {
        public CreateAmanaValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");
        }
    }
    public class CreateAmanaHandler : IRequestHandler<CreateAmanaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public CreateAmanaHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateAmanaCommand request, CancellationToken cancellationToken)
        {
            var amana = request.Adapt<Domain.Entities.Benaa.Amana>();
            amana.AreaId = 1;

            var maxId = await _context._tblAlamanat.MaxAsync(e => e.ID);
            amana.ID = maxId + 1;
            await _context.CreateAsync(amana, cancellationToken);
            //await _context._tblAlamanat.AddAsync(amana, cancellationToken);
            return Result.Successed(amana.Adapt<AmanaDto>());
        }
    }
}
