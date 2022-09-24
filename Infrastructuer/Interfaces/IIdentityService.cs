using System.Threading.Tasks;
using Common;
using Domain.Entities.Auth;

namespace Infrastructure {
  public interface IIdentityService {
    Task<Result> RegisterAsync(string email, string password);
    Task<Result> LoginAsync(string email, string password);
    Task<Result> RefreshTokenAsync(string token, string refreshToken);
    Task<Result> LoginWithFacebookAsync(string accessToken);
    Task<Result> ChangePasswordAsync(string passworrd, string newPassword);
    Task<Result> ForgetPasswordAsync(string Email);
    Task<Result> ResetPasswordAsync(string token, string email, string password);
    Task<Result> GenerateAuthenticationResultForUserAsync(User user,RefreshToken rToken=null);
  }
}