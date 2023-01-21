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
using Application.Office.Dtos;
using Infrastructure;
using Application.UserManagment.Commands;
using Application.UserManagment.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Office.Commands
{
    public class CreateOfficeCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public FileDto Image { get; set; }
        public string AmanaId { get; set; }

    }

    public class CreateOfficeValidator : AbstractValidator<CreateOfficeCommand>
    {
        public CreateOfficeValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");

            RuleFor(r => r.AmanaId).NotEmpty()
                  .WithMessage("AmanaId is Required");
        }
    }

    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IFileService fileService;

        public CreateOfficeHandler(IAppDbContext context, IFileService fileService)
        {
            _context = context;
            this.fileService = fileService;
        }

        public async Task<Result> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = request.Adapt<Domain.Entities.Benaa.Office>();
            await _context.CreateAsync(office, cancellationToken);

            if (request.Image.FileStatus != FileStatus.None)
            {
                var image = await fileService.SaveFileAsync(request.Image);
                office.ImageUrl = image.Url;
            }


            var client = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id == request.OwnerId);
            client.OfficeId = office.Id;
            _context.AppUsers.Update(client);

            return Result.Successed(office.Adapt<OfficeDto>());
        }
    }
}
