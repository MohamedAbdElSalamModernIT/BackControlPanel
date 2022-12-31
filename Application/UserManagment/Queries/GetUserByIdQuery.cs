using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Queries
{
    public class GetUserByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
        class Handler : IRequestHandler<GetUserByIdQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.AppUsers.Protected()
                  
                  .Include(u => u.UserRoles)
                  .ThenInclude(ur => ur.Role)
                  .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                var client = await _context.tblClients
               .FirstOrDefaultAsync(u => u.IdentityId == request.Id, cancellationToken);

                user.AmanaId = client?.AmanaId ?? null;
                user.OfficeName = client?.OfficeName;
                user.BaladiaId = client?.BaladiaId ?? null;
       


                return Result.Successed(user);
            }

        }
    }
}