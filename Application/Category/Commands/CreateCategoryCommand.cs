using Application.Category.Dtos;
using Common;
using Common.Exceptions;
using Common.Infrastructures;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;


namespace Application.Category.Commands
{
    public class CreateCategoryCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string ParentCategoryId { get; set; }
    }

    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                .WithMessage("Name is Required");
        }
    }

    public class CreateAttributeHandler : IRequestHandler<CreateCategoryCommand, Result>
    {
        private readonly IAppDbContext _context;

        public CreateAttributeHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var parentCategory = await _context.tblCategories.FirstOrDefaultAsync(
                e => e.Id == request.ParentCategoryId);

            if (parentCategory == null) return Result.Failure(ApiExceptionType.BadRequest);

            var category = request.Adapt<Domain.Entities.Benaa.Category>();

            await _context.CreateAsync(category, cancellationToken);
            category.ParentCategory = parentCategory;
            return Result.Successed(category.Adapt<CategoryDto>());
        }
    }
}
