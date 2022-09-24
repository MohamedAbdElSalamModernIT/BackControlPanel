using System.Threading;
using System.Threading.Tasks;
using Common;
using Infrastructure;
using MediatR;

namespace Application.Auth.Commands {
    public class ForgetPasswordCommand : IRequest<Result> {

        public string Email { get; set; }
        class Handler : IRequestHandler<ForgetPasswordCommand, Result> {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService) {
                _identityService = identityService;
            }




            public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken) {
                return await _identityService.ForgetPasswordAsync(request.Email);
            }
        }
    }
}
