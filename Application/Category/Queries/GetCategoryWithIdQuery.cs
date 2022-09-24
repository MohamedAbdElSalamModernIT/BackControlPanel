using Application.Category.Dtos;
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using System.Linq;

namespace Application.Category.Queries
{
    public class GetCategoryWithIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
    }

    public class GetCategoryWithIdQueryValidator : AbstractValidator<GetCategoryWithIdQuery>
    {
        public GetCategoryWithIdQueryValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull()
                .WithMessage("Id is Required");
        }
    }


    public class GetCategoryWithIdQueryHandler : IRequestHandler<GetCategoryWithIdQuery, Result>
    {
        private readonly IAppDbContext _context;

        public GetCategoryWithIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetCategoryWithIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.tblCategories
                .Include(e => e.ParentCategoryId)
                .Where(e => e.Id == request.Id)
                .ProjectToType<CategoryDto>().FirstOrDefaultAsync(cancellationToken);

            if (category == null) return Result.Failure(ApiExceptionType.NotFound);

            return Result.Successed(category);
        }
    }
}
