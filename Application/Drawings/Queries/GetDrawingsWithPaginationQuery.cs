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
        public string ClientId { get; set; }
        public string OfficeName { get; set; }
        public int? BuildingTypeId { get; set; }
        public int? BaladiaId { get; set; }
        public int? RequestNo{ get; set; }
        public DrawingStatus? Status { get; set; }
        public FileType? FileType { get; set; }
        public DrawingType? DrawingType { get; set; }
    }
    public class GetDrawingsWithPaginationHandler : IRequestHandler<GetDrawingsWithPaginationQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetDrawingsWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetDrawingsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblDrawings
                .Include(e => e.Baladia)
                .Include(e => e.BuildingType)
                .Include(e => e.Client)
                .AsQueryable();



            if (!string.IsNullOrEmpty(request.ClientId))
                query = query.Where(e => e.ClientId == request.ClientId);

            if (!string.IsNullOrEmpty(request.OfficeName))
                query = query.Where(e => e.Client.OfficeName.Contains(request.OfficeName));

            if (request.BuildingTypeId.HasValue)
                query = query.Where(e => e.BuildingTypeId == request.BuildingTypeId.Value);

            if (request.Status.HasValue)
                query = query.Where(e => e.Status == request.Status.Value);

            if (request.RequestNo.HasValue)
                query = query.Where(e => e.RequestNo == request.RequestNo.Value);

            if (request.BaladiaId.HasValue)
                query = query.Where(e => e.BaladiaId == request.BaladiaId.Value);

            if (request.FileType.HasValue)
                query = query.Where(e => e.FileType == request.FileType.Value);

            if (request.DrawingType.HasValue)
                query = query.Where(e => e.DrawingType == request.DrawingType.Value);

            var drawings = await query.ProjectToType<DrwaingDto>()
                .OrderByDescending(e=>e.CreatedDate)
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(drawings);
        }
    }
}
