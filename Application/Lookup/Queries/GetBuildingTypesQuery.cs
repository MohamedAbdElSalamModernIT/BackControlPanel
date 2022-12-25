using Application.Conditions.Dtos;
using Common;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using Application.Lookup.Dtos;

namespace Application.Lookup.Queries
{
    public class GetBuildingTypesQuery : IRequest<Result>
    {
    }
    public class GetBuildingTypesQueryHandler : IRequestHandler<GetBuildingTypesQuery, Result>
    {
        private readonly IAppDbContext _context;

        public GetBuildingTypesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetBuildingTypesQuery request, CancellationToken cancellationToken)
        {
            var buildingTypes = await _context.tblBuildingTypes.ToListAsync(cancellationToken);

            return Result.Successed(buildingTypes);
        }
    }
   
}
