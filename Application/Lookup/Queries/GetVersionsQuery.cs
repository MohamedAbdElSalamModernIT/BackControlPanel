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
using Application.Lookup.Dtos;

namespace Application.Lookup.Queries
{
    public class GetVersionsQuery : IRequest<Result>
    {
    }

    public class GetVersionsQueryHandler : IRequestHandler<GetVersionsQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetVersionsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetVersionsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblVersions
                .AsQueryable();

            var list = await query.ProjectToType<VersionDto>()
            .ToListAsync(cancellationToken);

            return Result.Successed(list);
        }
    }


}
