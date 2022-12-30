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
using Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Application.Drawings.Queries
{
    public class GetDrawingFileQuery : IRequest<Result>
    {
        public string DrawingId { get; set; }
    }
    public class GetDrawingFileQueryHandler : IRequestHandler<GetDrawingFileQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IExcelService excelService;

        public GetDrawingFileQueryHandler(IAppDbContext context, IExcelService excelService)
        {
            _context = context;
            this.excelService = excelService;
        }

        public async Task<Result> Handle(GetDrawingFileQuery request, CancellationToken cancellationToken)
        {

            var drawing = await _context.tblDrawings
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.Id == request.DrawingId);

            var bytes = excelService.GenerateExcell(drawing.File);

            return Result.Successed(new AppFile(bytes, drawing.Client.OfficeName + drawing.Extension));
        }

    }
}
