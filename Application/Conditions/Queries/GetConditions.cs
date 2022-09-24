using Application.Category.Dtos;
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using System.Linq;
using System;
using Application.Conditions.Dtos;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Domain.Entities.Benaa;
using Common.Infrastructures;
using Common.Extensions;

namespace Application.Conditions.Queries
{
    public class GetConditions : Paging, IRequest<Result>
    {
        public int Id { get; set; } = 0;
        public int BuildingTypeId { get; set; } = 0;
        public int BaladyaId { get; set; } = 0;
        public int AmanaId { get; set; } = 0;
        public string CategoryId { get; set; }
        public string ParentCategory { get; set; }
        public string Condition { get; set; }
        public int VersionNo { get; set; } = 0;
        public class GetConditionsHandler : IRequestHandler<GetConditions, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IXmlService xmlService;

            public GetConditionsHandler(IAppDbContext context, IXmlService xmlService)
            {
                _context = context;
                this.xmlService = xmlService;
            }

            public async Task<Result> Handle(GetConditions request, CancellationToken cancellationToken)
            {
                var query = _context.tblConditionsMap
                    .Include(e => e.BuildingType)
                    .Include(e => e.Condition).ThenInclude(e => e.Category).ThenInclude(e => e.ParentCategory)
                    .Include(e => e.Condition).ThenInclude(e => e.Version)
                    .Include(e => e.Baladia).ThenInclude(e => e.Amana)
                    .Where(e => e.AlBaladiaID != 1)
                    .AsQueryable();

                if (request.Id != 0)
                {
                    query = query.Where(e => e.Condition.ID == request.Id);
                }
                if (request.BuildingTypeId != 0)
                {
                    query = query.Where(e => e.BuildingTypeID == request.BuildingTypeId);
                }
                if (request.BaladyaId != 0)
                {
                    query = query.Where(e => e.AlBaladiaID == request.BaladyaId);
                }
                if (request.AmanaId != 0)
                {
                    query = query.Where(e => e.Baladia.AmanaId == request.AmanaId);
                }
                if (request.VersionNo != 0)
                {
                    query = query.Where(e => e.Condition.VersionId == request.VersionNo);
                }
                if (!string.IsNullOrEmpty(request.CategoryId))
                {
                    query = query.Where(e => e.Condition.CategoryId == request.CategoryId);
                }
                if (!string.IsNullOrEmpty(request.ParentCategory))
                {
                    query = query.Where(e => e.Condition.Category.ParentCategoryId == request.ParentCategory);
                }

                if (!string.IsNullOrEmpty(request.Condition))
                {
                    query = query.Where(e => e.Condition.Description.Contains(request.Condition));
                }
             

                var conditions = await query
                .ToPagedListAsync(request, cancellationToken);

                var items = conditions.Items
                    .Select(s =>
                    {
                        var parameters = xmlService.GetNodes(s.ParametersValues);

                        var description = s.Condition.Description;
                        foreach (var item in parameters)
                        {
                            description = description.Replace(item.Name, item.Value);
                        }
                        var result = s.Adapt<ConditionsMapDto>();
                        result.Condition = description;
                        return result;
                    }).ToList();

                return Result.Successed(new { Metadata = conditions.Metadata, Items = items });
            }
        }
    }
}
