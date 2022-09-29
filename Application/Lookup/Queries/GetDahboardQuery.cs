using Application.Lookup.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Lookup.Queries
{
    public class GetDahboardQuery :  IRequest<Result>
    {
    }

    public class GetDahboardQueryHandler : IRequestHandler<GetDahboardQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetDahboardQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetDahboardQuery request, CancellationToken cancellationToken)
        {
            var counts = (from m in _context.tblAlBaladiat
                          let Baladyat = _context.tblAlBaladiat.Count(e => e.ID != 1)
                          let Amanat = _context._tblAlamanat.Count()
                          let Categories = _context.tblCategories.Count(e=>e.ParentCategoryId!=null)
                          let Conditions = _context.tblConditions.Count()
                          let Users = _context.AppUsers.Count()
                          select new { Baladyat, Amanat, Categories, Conditions , Users }).Take(1);

            return Result.Successed(counts);
        }
    }
}
