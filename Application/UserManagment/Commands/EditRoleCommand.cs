using System;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Domain.Entities.Auth;
using Domain.Enums.Roles;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Commands {
  public class EditRoleCommand :IRequest<Result>{
        public string Id { get; set; }
        public string Name { get; set; }

        public string[] Permissions { get; set; }
    }

    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, Result> {
        private readonly IAppDbContext _context;
        private readonly IPermissionService _service;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EditRoleCommandHandler(IAppDbContext context, IMapper mapper, IPermissionService service,IMediator mediator) {
            _context = context;
            _mapper = mapper;
            _service = service;
            _mediator = mediator;
        }
        public async Task<Result> Handle(EditRoleCommand request, CancellationToken cancellationToken) {

            var role = await _context.Set<Role>().FindAsync(request.Id);
            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();


            _context.Set<Role>().Update(role);

            role.Permissions = null;

            if (request.Permissions.Length > 0)
                await _service.AddPermissionsToRole(role, request.Permissions);
            
            
            return Result.Successed(_mapper.Map<RoleDto>(role));
        }
    }
}
