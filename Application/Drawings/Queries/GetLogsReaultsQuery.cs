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
    public class GetLogsReaultsQuery : IRequest<Result>
    {
        public string LogId { get; set; }
    }
    public class GetLogsReaultsQueryHandler : IRequestHandler<GetLogsReaultsQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IExcelService excelService;

        public GetLogsReaultsQueryHandler(IAppDbContext context, IExcelService excelService)
        {
            _context = context;
            this.excelService = excelService;
        }

        public async Task<Result> Handle(GetLogsReaultsQuery request, CancellationToken cancellationToken)
        {

            var logs = await _context.tblConditionResults
                .Where(e => e.LogId == request.LogId)
                .ProjectToType<ConditionResultDto>()
                .ToListAsync(cancellationToken);

            var bytes = excelService.GenerateExcell(logs);

            return Result.Successed(bytes);
        }
    }
}
