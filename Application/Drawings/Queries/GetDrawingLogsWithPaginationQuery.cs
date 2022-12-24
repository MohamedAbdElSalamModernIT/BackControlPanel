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

namespace Application.DrawingLogs.Queries
{
    public class GetDrawingLogsWithPaginationQuery : Paging, IRequest<Result>
    {
        public string DrawingId { get; set; }
    }
    public class GetDrawingLogsWithPaginationHandler : IRequestHandler<GetDrawingLogsWithPaginationQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetDrawingLogsWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetDrawingLogsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblDrawingLogs
                .Where(e => e.DrwaingId == request.DrawingId)
                .AsQueryable();

            var drawingLogs = await query.ProjectToType<DrwaingLogDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(drawingLogs);
        }
    }
}
