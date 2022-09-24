using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Commands {
  public class DeleteRoleCommand : IRequest<Result> {

        public string Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result> {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;


        public DeleteRoleCommandHandler(IAppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken) {

            var role = await _context.Set<Role>().FindAsync(request.Id);
            if (role == null) {
                throw new ApiException(ApiExceptionType.NotFound);
            }
            _context.Set<Role>().Remove(role);
            return Result.Successed(_mapper.Map<RoleDto>(role));

        }
    }
}
