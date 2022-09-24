using System.Threading;
using System.Threading.Tasks;
using Common;
using Infrastructure;
using MediatR;

namespace Application.Auth.Commands {
  public class RefreshTokenCommand:IRequest<Result>
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }

    class Handler : IRequestHandler<RefreshTokenCommand, Result> {
      private readonly IIdentityService _identityService;

      public Handler(IIdentityService identityService) {
        _identityService = identityService;
      }
      public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        return await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
      }
    }
  }
}