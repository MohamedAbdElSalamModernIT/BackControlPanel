using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Commands {
   public class AddRoleCommand :IRequest<Result> {
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, Result> {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPermissionService _service;


        public AddRoleCommandHandler(IAppDbContext context,IMapper mapper, IPermissionService service) {
            _context = context;
            _mapper = mapper;
            _service = service;
        }
        public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken) {

            var role = new Role(request.Name) {
                NormalizedName = request.Name.ToUpper(),
                
            };
         
            await  _context.Set<Role>().AddAsync(role,cancellationToken);

            if (request.Permissions.Length > 0)
            {
               await _service.AddPermissionsToRole(role, request.Permissions);
            }
            return Result.Successed(_mapper.Map<RoleDto>(role));
        }
    }
}
