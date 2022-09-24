using Application.Baladya.Dto;
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

namespace Application.Baladya.Queries
{
    public class GetBaladyatWithPagination : Paging, IRequest<Result>
    {

    }

    public class GetBaladyatWithPaginationHandler : IRequestHandler<GetBaladyatWithPagination, Result>
    {
        private readonly IAppDbContext _context;

        public GetBaladyatWithPaginationHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetBaladyatWithPagination request, CancellationToken cancellationToken)
        {
            var query = _context.tblAlBaladiat.Where(e => e.ID != 1)
                .Include(e => e.Amana).AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(e => e.Name.Contains(request.Filter));

            var baldyat = await query.ProjectToType<BaladyaDto>()
                .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(baldyat);
        }
    }
}