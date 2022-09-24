using System.Threading;
using System.Threading.Tasks;
using Common;
using Infrastructure;
using MediatR;

namespace Application.Auth.Commands
{
  public class LoginCommand:IRequest<Result>
  {
    public string Username { get; set; }
    public string Password { get; set; }

    class Handler : IRequestHandler<LoginCommand, Result> {
      private readonly IIdentityService _identityService;

      public Handler(IIdentityService identityService) {
        _identityService = identityService;
      }
      public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken) {
        return await _identityService.LoginAsync(request.Username, request.Password);
      }
    }
  }
}
