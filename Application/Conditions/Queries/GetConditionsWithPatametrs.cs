using Application.Conditions.Dtos;
using Common;
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
    public class GetConditionsWithPatametrs : Paging, IRequest<Result>
    {
    }
    public class GetConditionsWithPatametrsHandler : IRequestHandler<GetConditionsWithPatametrs, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public GetConditionsWithPatametrsHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(GetConditionsWithPatametrs request, CancellationToken cancellationToken)
        {
            var list = await _context.tblConditionsMap
                    .Include(e => e.Condition)
                    .Where(e => e.AlBaladiaID == 1)
                    .ProjectToType<ConditionWithParameters>()
                    .ToListAsync();

            list = list.Select(e =>
            {
                e.Parameters = xmlService.GetNodes(e.ParametersValues);
                return e;
            }).ToList();

            return Result.Successed(list);
        }
    }
}
