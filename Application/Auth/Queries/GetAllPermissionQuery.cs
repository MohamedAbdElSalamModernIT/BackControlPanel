using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Auth.Queries {
    public class GetAllPermissionQuery : IRequest<Result> {

    class Handler : IRequestHandler<GetAllPermissionQuery, Result> {
        private readonly IPermissionService _service;

        public Handler(IPermissionService service) {
            this._service = service;
        }
        public  Task<Result> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken) {
            return Task.FromResult( Result.Successed(_service.GetPermissions()));
        }
    }
    }

}
