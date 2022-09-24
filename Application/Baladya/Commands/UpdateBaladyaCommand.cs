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
    public class UpdateBaladyaCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int AmanaId { get; set; }
        public string Name { get; set; }
    }
    public class UpdateBaladyaValidator : AbstractValidator<UpdateBaladyaCommand>
    {
        public UpdateBaladyaValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                 .WithMessage("Id is Required");
            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");
            
            RuleFor(r => r.AmanaId).NotEmpty()
                  .WithMessage("AmanaId is Required");
        }
    }
    public class UpdateBaladyaHandler : IRequestHandler<UpdateBaladyaCommand, Result>
    {
        private readonly IAppDbContext _context;

        public UpdateBaladyaHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateBaladyaCommand request, CancellationToken cancellationToken)
        {
            var amana = await _context._tblAlamanat.FirstOrDefaultAsync(e => e.ID == request.AmanaId);
            if (amana == null) return Result.Failure(ApiExceptionType.BadRequest);
            var baladya = await _context.tblAlBaladiat
                .FirstOrDefaultAsync(e => e.ID == request.Id, cancellationToken);

            if (baladya == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedBaladya = request.Adapt(baladya);

            _context.Edit(updatedBaladya);
            baladya.Amana = amana;

            return Result.Successed(updatedBaladya.Adapt<BaladyaDto>());
        }
    }
}
