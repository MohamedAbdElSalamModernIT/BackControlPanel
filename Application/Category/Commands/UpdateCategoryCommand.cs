using Application.Category.Dtos;
using Common;
using Common.Infrastructures;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;


namespace Application.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentCategoryId { get; set; }

       
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull()
                .WithMessage("Id is Required");

            RuleFor(r => r.Name).NotEmpty().NotNull()
               .WithMessage("Name is Required");

       
        }
    }

    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IAppDbContext _context;
       

        public UpdateCategoryHandler(IAppDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categry = await _context.tblCategories.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (categry == null) return Result.Failure(ApiExceptionType.NotFound);

            var updatedCategory = request.Adapt(categry);

            _context.Edit(updatedCategory);

            return Result.Successed(updatedCategory.Adapt<CategoryDto>());
        }
    }
}
