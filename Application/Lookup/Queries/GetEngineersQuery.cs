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
using Domain.Enums;

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


    public class GetOfficeManagersQuery : IRequest<Result>
    {
    }
    public class GetOfficeManagersQueryHandler : IRequestHandler<GetOfficeManagersQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public GetOfficeManagersQueryHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(GetOfficeManagersQuery request, CancellationToken cancellationToken)
        {

            var query = _context.AppUsers
                .Where(e => string.IsNullOrEmpty(e.OfficeId))
                .Where(e => e.UserType == UserType.OfficeManager)
                .AsQueryable();

            if (!string.IsNullOrEmpty(auditService.AmanaId))
            {
                int amanaId = int.Parse(auditService.AmanaId);
                query = query.Where(e => e.AmanaId == amanaId);
            }
            var users = await query
                .Select(e => new { e.FullName, e.Id })
                .ToListAsync(cancellationToken);

            return Result.Successed(users);
        }
    }

}
