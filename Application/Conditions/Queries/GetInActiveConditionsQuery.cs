using Application.Conditions.Dtos;
using Common;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Conditions.Queries
{
    public class GetInActiveConditionsQuery : IRequest<Result>
    {
    }
    public class GetInActiveConditionsQueryHandler : IRequestHandler<GetInActiveConditionsQuery, Result>
    {
        private readonly IAppDbContext _context;


        public GetInActiveConditionsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetInActiveConditionsQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.tblConditions
                    .Where(e => !e.Active)
                    .Select(e => e.ID)
                    .ToListAsync();

            return Result.Successed(list);
        }
    }
}
