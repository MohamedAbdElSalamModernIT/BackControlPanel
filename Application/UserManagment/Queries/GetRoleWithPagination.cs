using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Queries
{
    public class GetRoleWithPagination : Paging, IRequest<Result> { }

    public class GetRoleWithPaginationHandler : IRequestHandler<GetRoleWithPagination, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetRoleWithPaginationHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRoleWithPagination request, CancellationToken cancellationToken)
        {
            var query = _context.Set<Role>().AsQueryable();
            if (!String.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(r => r.Name.Contains(request.Filter));
            }
            var rules = await query.ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
              .ToPagedListAsync(request, cancellationToken);

            return Result.Successed(rules);
        }
    }
}