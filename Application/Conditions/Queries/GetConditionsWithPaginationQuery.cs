﻿using Application.Category.Dtos;
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
using Application.Conditions.Dtos;

namespace Application.Conditions.Queries
{
    public class GetConditionsWithPaginationQuery : Paging, IRequest<Result>
    {
        public string CategoryId { get; set; }
        public int Id { get; set; } = 0;
    }
    public class GetConditionsWithPaginationHandler : IRequestHandler<GetConditionsWithPaginationQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetConditionsWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetConditionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblConditions
                .Include(e=>e.Category)
                .Include(e=>e.Version)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query= query.Where(e => e.Description.Contains(request.Filter));

            if (request.Id != 0)
            {
                query = query.Where(e => e.ID == request.Id);
            }

            if (!string.IsNullOrEmpty(request.CategoryId))
                query = query.Where(e => e.CategoryId == request.CategoryId);

            var Conditions = await query.ProjectToType<ConditionDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(Conditions);
        }
    }
}
