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
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Queries
{
    public class GetUserListQuery : Paging, IRequest<Result>
    {
        class Handler : IRequestHandler<GetUserListQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(GetUserListQuery request, CancellationToken cancellationToken)
            {
                var query = _context.AppUsers.Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role).AsQueryable();

                if (!string.IsNullOrEmpty(request.Filter))
                    query = query.Where(e => e.UserName.Contains(request.Filter));

                var users = await query.ProjectToType<UserDto>()
                 .ToPagedListAsync(request, cancellationToken);

                return Result.Successed(users);
            }
        }
    }
}
