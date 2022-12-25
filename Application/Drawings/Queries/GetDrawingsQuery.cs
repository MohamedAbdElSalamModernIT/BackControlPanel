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
using Application.Drawings.Dto;

namespace Application.Drawings.Queries
{
    public class GetDrawingsQuery : IRequest<Result>
    {
    }
    public class GetDrawingsHandler : IRequestHandler<GetDrawingsQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public GetDrawingsHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(GetDrawingsQuery request, CancellationToken cancellationToken)
        {
            var drawings = await _context.tblDrawings
                .Include(e => e.Baladia).ThenInclude(e=>e.Amana)
                .Include(e => e.BuildingType)
                .Where(e => e.ClientId == auditService.UserId)
                .ProjectToType<DrwaingPluginDto>()
                .ToListAsync(cancellationToken);

            return Result.Successed(drawings);
        }
    }
}
