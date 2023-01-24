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
using Microsoft.Extensions.DependencyInjection;

namespace Application.Office.Commands
{
    public class UpdateOfficeCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public FileDto Image { get; set; }
    }

    public class UpdateOfficeValidator : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                  .WithMessage("Id is Required");

            RuleFor(r => r.Name).NotEmpty()
                  .WithMessage("Name is Required");

            RuleFor(r => r.PhoneNumber).NotEmpty()
                  .WithMessage("PhoneNumber is Required");

            RuleFor(r => r.Email).NotEmpty()
                  .WithMessage("Email is Required");

        }
    }

    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IFileService fileService;

        public UpdateOfficeHandler(IAppDbContext context, IImageService imageService
            , IFileService fileService)
        {
            _context = context;
            this.fileService = fileService;
        }

        public async Task<Result> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _context.tblOffices
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (office == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedoffice = request.Adapt(office);

            if (request.Image.FileStatus != FileStatus.None)
            {
                var image = await fileService.SaveFileAsync(request.Image);
                office.ImageUrl = image.Url;
            }

            _context.Edit(updatedoffice);

            return Result.Successed(office.Adapt<OfficeDto>());
        }
    }
}
