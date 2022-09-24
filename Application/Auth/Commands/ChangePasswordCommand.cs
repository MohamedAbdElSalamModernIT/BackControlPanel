using System.Threading;
using System.Threading.Tasks;
using Common;
using Infrastructure;
using MediatR;

namespace Application.Auth.Commands
{
   public class ChangePasswordCommand:IRequest<Result>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        class Handler : IRequestHandler<ChangePasswordCommand, Result>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
        

            public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                return await _identityService.ChangePasswordAsync(request.OldPassword, request.NewPassword);
            }
        }
    }
}
