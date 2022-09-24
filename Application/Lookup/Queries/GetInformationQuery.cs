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
using Application.Lookup.Dtos;

namespace Application.Lookup.Queries
{

    public class GetInformationQuery : Paging, IRequest<Result>
    {
    }
    public class GetAmanatQueryHandler : IRequestHandler<GetInformationQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetAmanatQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetInformationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.tblInformation
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query= query.Where(e => e.Description.Contains(request.Filter));

            var list = await query.ProjectToType<InformationDto>()
            .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(list);
        }
    }
   
}
