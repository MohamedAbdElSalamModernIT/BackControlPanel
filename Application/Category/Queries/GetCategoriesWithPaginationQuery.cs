using Application.Category.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Category.Queries
{
    public class GetCategoriesWithPaginationQuery : Paging, IRequest<Result>
    {
        public bool? ParentOnly { get; set; }
        public string ParentCategory{ get; set; }
    }
    public class GetCategoriesWithPaginationHandler : IRequestHandler<GetCategoriesWithPaginationQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetCategoriesWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblCategories
                .Include(c => c.ParentCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            if (!string.IsNullOrEmpty(request.ParentCategory))
                query = query.Where(e => e.ParentCategoryId == request.ParentCategory);

            if (request.ParentOnly.HasValue)
                query = query.Where(e => (e.ParentCategoryId == null) == request.ParentOnly);

            var categories = await query.ProjectToType<CategoryDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(categories);
        }
    }
}
