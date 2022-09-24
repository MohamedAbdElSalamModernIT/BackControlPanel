using Application.Lookup.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Lookup.Queries
{
    public class GetBaladyatByAmanaIdQuery : Paging, IRequest<Result>
    {
        public int AmanaId { get; set; }
    }
    public class GetBaladyatByAreaIdQueryHandler : IRequestHandler<GetBaladyatByAmanaIdQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetBaladyatByAreaIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetBaladyatByAmanaIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblAlBaladiat
                .Where(e => e.AmanaId == request.AmanaId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            var list = await query.ProjectToType<AmanaDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(list);
        }
    }
}
