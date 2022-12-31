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
        public string FullName { get; set; }
        public string Username { get; set; }
        public int BaladyaId { get; set; } = 0;
        public int AmanaId { get; set; } = 0;
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
                var query = _context.AppUsers
                    .Protected()
                    .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role).AsQueryable();

                //if (request.BaladyaId != 0)
                //{
                //    query = query.Where(e => e.AlBaladiaID == request.BaladyaId);
                //}
                //if (request.AmanaId != 0)
                //{
                //    query = query.Where(e => e.Baladia.AmanaId == request.AmanaId);
                //}

                if (!string.IsNullOrEmpty(request.FullName))
                    query = query.Where(e => e.FirstName.Contains(request.FullName));
                
                if (!string.IsNullOrEmpty(request.FullName))
                    query = query.Where(e => e.LastName.Contains(request.FullName));
                
                if (!string.IsNullOrEmpty(request.Username))
                    query = query.Where(e => e.UserName.Contains(request.Username));

                var users = await query.ProjectToType<UserDto>()
                    .OrderByDescending(e=>e.CreatedDate)
                 .ToPagedListAsync(request, cancellationToken);

                return Result.Successed(users);
            }
        }
    }
}
