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
    public class GetDrawingsWithPaginationQuery : Paging, IRequest<Result>
    {
        public string EngineerId { get; set; }
        public string OfficeId { get; set; }
        public string CustomerName { get; set; }
        public int? BuildingTypeId { get; set; }
        public int? BaladiaId { get; set; }
        public int? AmanaId { get; set; }
        public int? RequestNo { get; set; }
        public DrawingStatus? Status { get; set; }
        public OfficeDrawingStatus? OfficeStatus { get; set; }
        public FileType? FileType { get; set; }
        public DrawingType? DrawingType { get; set; }
    }
    public class GetDrawingsWithPaginationHandler : IRequestHandler<GetDrawingsWithPaginationQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public GetDrawingsWithPaginationHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(GetDrawingsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblDrawings
                .Protected()
                .Include(e => e.Baladia)
                .Include(e => e.Office)
                .Include(e => e.BuildingType)
                .Include(e => e.Engineer)
                .AsQueryable();

            var initailOfficeId = !string.IsNullOrEmpty(auditService.OfficeId) ? auditService.OfficeId : request.OfficeId;


            if (!string.IsNullOrEmpty(request.EngineerId))
                query = query.Where(e => e.EngineerId == request.EngineerId);

            if (!string.IsNullOrEmpty(initailOfficeId))
                query = query.Where(e => e.OfficeId == initailOfficeId);

            if (!string.IsNullOrEmpty(request.CustomerName))
                query = query.Where(e => e.CustomerName.Contains(request.CustomerName));

            if (request.BaladiaId.HasValue)
                query = query.Where(e => e.BaladiaId == request.BaladiaId.Value);

            if (request.AmanaId.HasValue)
                query = query.Where(e => e.Baladia.AmanaId == request.AmanaId);

            if (request.BuildingTypeId.HasValue)
                query = query.Where(e => e.BuildingTypeId == request.BuildingTypeId.Value);

            if (request.OfficeStatus.HasValue)
                query = query.Where(e => e.OfficeStatus == request.OfficeStatus.Value);

            if (request.Status.HasValue)
                query = query.Where(e => e.Status == request.Status.Value);

            if (request.RequestNo.HasValue)
                query = query.Where(e => e.RequestNo == request.RequestNo.Value);


            if (request.FileType.HasValue)
                query = query.Where(e => e.FileType == request.FileType.Value);

            if (request.DrawingType.HasValue)
                query = query.Where(e => e.DrawingType == request.DrawingType.Value);

            var drawings = await query.ProjectToType<DrwaingDetailsDto>()
                .OrderByDescending(e => e.CreatedDate)
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(drawings);
        }
    }
}
