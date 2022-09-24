using System.Threading.Tasks;
using Application.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
          [FromBody] LoginCommand loginDto
        )
        {
            return Ok((await Mediator.Send(loginDto)));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePassword)
        {
            return Ok((await Mediator.Send(changePassword)));
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand forgetPassword)
        {
            return Ok((await Mediator.Send(forgetPassword)));

        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPassword)
        {
            return Ok((await Mediator.Send(resetPassword)));

        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            return Ok((await Mediator.Send(command)));
        }
    }
}
