using Application.Office.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Office.Queries
{
    public class GetAllOfficesWithPagination : Paging, IRequest<Result>
    {

    }

    public class GetAmanatWithPaginationHandler : IRequestHandler<GetAllOfficesWithPagination, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;

        public GetAmanatWithPaginationHandler(IAppDbContext context, IAuditService auditService)
        {
            _context = context;
            this.auditService = auditService;
        }

        public async Task<Result> Handle(GetAllOfficesWithPagination request, CancellationToken cancellationToken)
        {
            var query = _context.tblOffices
               .Include(e => e.Owner).Include(e => e.Amana)
               .Protected()
                .AsQueryable();

            if (!string.IsNullOrEmpty(auditService.AmanaId))
            {
                var amanaId = int.Parse(auditService.AmanaId);
                query =  query.Where(e => e.AmanaId == amanaId);
            }

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            var offices = await query.ProjectToType<OfficeDto>()
                .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(offices);
        }
    }
}
