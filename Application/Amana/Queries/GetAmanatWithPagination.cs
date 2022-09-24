using Application.Amana.Dto;

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

namespace Application.Amana.Queries
{
    public class GetAmanatWithPagination : Paging, IRequest<Result>
    {

    }

    public class GetAmanatWithPaginationHandler : IRequestHandler<GetAmanatWithPagination, Result>
    {
        private readonly IAppDbContext _context;

        public GetAmanatWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAmanatWithPagination request, CancellationToken cancellationToken)
        {
            var query = _context._tblAlamanat.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            var amanat = await query.ProjectToType<AmanaDto>()
                .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(amanat);
        }
    }
}