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
using Domain.Enums;

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

            var query = _context.tblDrawings
                .Protected()
                .Include(e => e.Baladia).ThenInclude(e => e.Amana)
                .Include(e => e.BuildingType)
                .Include(e => e.Engineer)
                .Include(e => e.Office)               
                .AsQueryable();

            var userType = (UserType)int.Parse(auditService.UserType);

            if (userType == UserType.Engineer)
                query = query.Where(e => e.EngineerId == auditService.UserId);
            else
                query = query.Where(e => e.OfficeId == auditService.OfficeId);

            var drawings = await query
                .ProjectToType<DrwaingPluginDto>()
                .ToListAsync(cancellationToken);

            return Result.Successed(drawings);
        }
    }
}
