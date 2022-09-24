using Application.Lookup.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Lookup.Queries
{
    public class GetAmanatByAreaIdQuery : Paging, IRequest<Result>
    {
    }
    public class GetAmanatByAreaIdQueryHandler : IRequestHandler<GetAmanatByAreaIdQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetAmanatByAreaIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAmanatByAreaIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context._tblAlamanat.Include(e => e.Area)
                .OrderBy(e => e.AreaId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            var list = await query.ProjectToType<AmanaDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(list);
        }
    }
}
