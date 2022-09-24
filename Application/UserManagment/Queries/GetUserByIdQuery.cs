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

namespace Application.UserManagment.Queries {
  public class GetUserByIdQuery :  IRequest<Result> {
    public string Id { get; set; }
    class Handler : IRequestHandler<GetUserByIdQuery, Result> {
      private readonly IAppDbContext _context;
      private readonly IMapper _mapper;

      public Handler(IAppDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
      }

      public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        var users = await _context.AppUsers.Protected()
          .Include(u=>u.UserRoles)
          .ThenInclude(ur=>ur.Role)
          .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync(u=>u.Id==request.Id, cancellationToken);
               
         

        return Result.Successed(users);
      }
    
    }
  }
}