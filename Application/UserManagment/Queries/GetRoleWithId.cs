using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Queries {
   public class GetRoleWithId :IRequest<Result> {
        public string Id { get; set; }
    }
    public class GetRoleWithIdHandler : IRequestHandler<GetRoleWithId, Result> {

        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetRoleWithIdHandler(IAppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
      
        }
        public async Task<Result> Handle(GetRoleWithId request, CancellationToken cancellationToken) {
            var role = await _context.Set<Role>().FindAsync(request.Id);

            if (role == null)
                throw new ApiException(ApiExceptionType.NotFound);

            return Result.Successed(_mapper.Map<RolePermissionDto>(role));
        }
    }
}
