using System.Threading;
using System.Threading.Tasks;
using Common;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands {
    public class AddPermissionToRolCommand : IRequest<Result> {
        public string[] Permissions { get; set; }
        public string RoleId { get; set; }


    }

    public class AddPermissionToRolCommandHandler : IRequestHandler<AddPermissionToRolCommand, Result> {
        private readonly IPermissionService _service;
        private readonly IAppDbContext context;


        public AddPermissionToRolCommandHandler(IPermissionService service, IAppDbContext context) {
            _service = service;
            this.context = context;

        }

        public async Task<Result> Handle(AddPermissionToRolCommand request, CancellationToken cancellationToken) {


            var role = await context.Set<Role>().FirstOrDefaultAsync(r => r.Id == request.RoleId);
            await _service.AddPermissionsToRole(role, request.Permissions);

            return Result.Successed();
        }


    }




}
