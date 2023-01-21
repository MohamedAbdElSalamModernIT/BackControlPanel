using Application.Conditions.Dtos;
using Common;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Application.Lookup.Dtos;

namespace Application.Lookup.Queries
{
    public class GetEngineersQuery : IRequest<Result>
    {
    }
    public class GetEngineersQueryHandler : IRequestHandler<GetEngineersQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public GetEngineersQueryHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(GetEngineersQuery request, CancellationToken cancellationToken)
        {
            var engineers = await _context.AppUsers
                .Where(e => e.OfficeId == auditService.OfficeId)
                .Where(e => e.Id != auditService.UserId)
                .Select(e => new { e.FullName, e.Id })
                .ToListAsync(cancellationToken);

            return Result.Successed(engineers);
        }
    }

}
