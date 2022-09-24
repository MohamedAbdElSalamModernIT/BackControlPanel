using Application.Conditions.Dtos;
using Common;
using Common.Exceptions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Conditions.Queries
{
    public class GetConditionMapQuery : IRequest<Result>
    {
        public int BuildingTypeId { get; set; }
        public int AlBaladiaID { get; set; }
        public double ConditionId { get; set; }
    }

    public class GetConditionMapQueryHandler : IRequestHandler<GetConditionMapQuery, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public GetConditionMapQueryHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(GetConditionMapQuery request, CancellationToken cancellationToken)
        {
            var condition = await _context.tblConditionsMap
                .Include(e => e.Condition).ThenInclude(e => e.Category)
                .ThenInclude(e => e.ParentCategory)
                .Include(e => e.BuildingType)
                .Include(e => e.Baladia).ThenInclude(e=>e.Amana)
                .Where(e => e.AlBaladiaID == request.AlBaladiaID
                && e.ConditionID == request.ConditionId
                && e.BuildingTypeID == request.BuildingTypeId
                ).FirstOrDefaultAsync(cancellationToken);

            if (condition == null) return Result.Failure(ApiExceptionType.NotFound);

            var conditionDto = condition.Adapt<ConditionsMapDto>();
            conditionDto.Parameters = xmlService.GetNodes(condition.ParametersValues);
            return Result.Successed(conditionDto);
        }
    }
}
