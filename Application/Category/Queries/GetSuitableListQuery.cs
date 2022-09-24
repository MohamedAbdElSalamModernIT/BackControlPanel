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

namespace Application.Category.Queries
{
    public class GetSuitableListQuery : IRequest<Result>
    {
        public string Id { get; set; }
        public int BaladyaId { get; set; }
    }

    public class GetSuitableListQueryValidator : AbstractValidator<GetSuitableListQuery>
    {
        public GetSuitableListQueryValidator()
        {
            RuleFor(r => r.BaladyaId).NotEmpty().NotNull()
                .WithMessage("BaladyaId is Required");
        }
    }


    public class GetSuitableListQueryHandler : IRequestHandler<GetSuitableListQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public GetSuitableListQueryHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(GetSuitableListQuery request, CancellationToken cancellationToken)
        {
            var result = new CategoriesResultDto();

            var categories = await _context.tblCategories
                .Where(e => e.ParentCategoryId == request.Id)
                .ProjectToType<CategoryDto>().ToListAsync();

            result.Categories = categories;
            if (categories.Count == 0)
            {
                var conditions = await GetConditions(request.Id, request.BaladyaId);
                result.Conditions = conditions;
                result.IsConditions = true;
            }
            return Result.Successed(result);
        }

        private async Task<List<ConditionsMapDto>> GetConditions(string id, int baladyaId)
        {
            var categoryConditions = await _context.tblConditions
                .Where(e => e.CategoryId == id).ToListAsync();

            var conditionMap = await _context.tblConditionsMap
                .Where(e => e.AlBaladiaID == baladyaId)
                //.Where(e=>categoryConditions.Contains(e))
                .Include(e => e.BuildingType)
                .ToListAsync();

            var conditions = conditionMap
                .Where(e => categoryConditions.Any(c => c.ID == e.ConditionID))
                .Select(s =>
                {
                    var parameters = xmlService.GetNodes(s.ParametersValues);
                    var condition = categoryConditions.FirstOrDefault(x => x.ID == s.ConditionID);

                    var description = condition.Description;
                    foreach (var item in parameters)
                    {
                        description = description.Replace(item.Name, item.Value);
                    }

                    var result = s.Adapt<ConditionsMapDto>();
                    result.Condition = description;
                    return result;

                }).ToList();

            return conditions;
        }
    }
}
