using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Extensions;
using Common.Interfaces;
using Common.Options;
using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using Domain.Enums;
using Domain.Enums.Roles;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuditService _auditService;
        private readonly Common.Interfaces.IUrlHelper _urlHelper;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPermissionService _permissionServices;
        private readonly JwtOption _jwtOption;
        private readonly IAppDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(UserManager<User> userManager,
          IAuditService auditService,
          Common.Interfaces.IUrlHelper urlHelper,
          RoleManager<Role> roleManager,
          IPermissionService permissionServices,
          JwtOption jwtOption,
          IAppDbContext context,
          TokenValidationParameters tokenValidationParameters
        )
        {
            _userManager = userManager;
            _auditService = auditService;
            _roleManager = roleManager;
            _jwtOption = jwtOption;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            _urlHelper = urlHelper;
            _permissionServices = permissionServices;
        }

        public Task<Result> RegisterAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            //var user = await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .Protected(u => u.Active)
              .FirstOrDefaultAsync(u => u.UserName.ToLower() == email.ToLower());


            if (user == null)
            {
                throw new ApiException(ApiExceptionType.NotFound);
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
                throw new ApiException(ApiExceptionType.InvalidLogin);

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<Result> RefreshTokenAsync(string token, string refreshToken)
        {
            ApiException error = null;
            var validatedToken = GetPrincipalFromToken(token);
            var storedRefreshToken = await _context.Set<RefreshToken>()
              .SingleOrDefaultAsync(e => e.Token == refreshToken);

            if (validatedToken == null)
            {
                storedRefreshToken.Invalidated = true;
                throw new ApiException(ApiExceptionType.Unauthorized, "Invalid Token");
            }

            if (storedRefreshToken == null)
            {
                throw new ApiException(ApiExceptionType.Unauthorized, "Refresh token not exist");
            }

            var userId = validatedToken.Claims.Single(e => e.Type == "UserId").Value;
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .Protected(u => u.Active)
              .FirstOrDefaultAsync(u => u.Id == userId);


            if (user == null)
            {
                throw new ApiException(ApiExceptionType.Unauthorized, "User not found");
            }

            if (storedRefreshToken.Invalidated)
            {
                error = new ApiException(ApiExceptionType.Unauthorized, "Invalid refresh token");
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                storedRefreshToken.Invalidated = true;
                error = new ApiException(ApiExceptionType.Unauthorized, "Refresh token expired");
            }
            if (storedRefreshToken.Used)
            {
                // error = new ApiException(ApiExeptionType.Unauthorized, "Refresh token used");
            }

            if (error != null)
            {
                throw error;
            }


            _context.RefreshToken.Update(storedRefreshToken);
            await _context.SaveChangesAsync();


            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // user Issuer = guid ;
                // accept token and user;
                //_tokenValidationParameters.ValidateIssuer = user.Issuer;

                _tokenValidationParameters.ValidateLifetime = false;

                var principals = tokenHandler.ValidateToken(token, _tokenValidationParameters,
                  out var validatedToken);
                _tokenValidationParameters.ValidateLifetime = true;
                return !ISJwtWithValidSecurityAlgorithm(validatedToken) ? null : principals;
            }
            catch
            {
                return null;
            }
        }


        private bool ISJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                     StringComparison.InvariantCultureIgnoreCase);
        }


        public Task<Result> LoginWithFacebookAsync(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> GenerateAuthenticationResultForUserAsync(User user, RefreshToken rToken = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshToken = rToken;
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            if (refreshToken == null)
                refreshToken = new RefreshToken
                {
                    JwtId = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6)
                };

            await _context.Set<RefreshToken>().AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return Result.Successed(new
            {
                Token = await GenerateJwtToken(user.Email, user),
                RefreshToken = refreshToken.Token,
                //User = user
            });
        }

        private async Task<string> GenerateJwtToken(string email, User appUser)
        {
            var permissions = _permissionServices.GetPermissionListForUser(appUser);

            var client = await _context.AppUsers
                .Include(e => e.Office)
                .FirstOrDefaultAsync(e => e.Id == appUser.Id);

            var amanId = client?.AmanaId.ToString() ?? "";
            var BaladiaId = client?.BaladiaId.ToString() ?? "";
            var officeName = client?.Office?.Name ?? "";
            var officeId = client?.Office?.Id ?? "";
            int userType = (int)client?.UserType;
            string userTypeStr = $"{userType}";



            var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, appUser.UserName),
        //new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, appUser.Id),
        new Claim("UserId", appUser.Id),
        new Claim("AmanId", amanId),
        new Claim("BaladiaId", BaladiaId),
        new Claim("UserType", userTypeStr),
        //new Claim("allowedModules", appUser.AllowedModules.ToString()),
        new Claim("permissions", JsonConvert.SerializeObject(permissions)),
        new Claim("email", appUser.UserName),
        new Claim("fullName", appUser.FullName),
        new Claim("officeName", officeName),
        new Claim("officeId", officeId),
        new Claim("role", JsonConvert.SerializeObject(appUser.UserRoles.Select(s => s.Role.Name))),

      };




            var userClaims = await _userManager.GetClaimsAsync(appUser);
            //claims.AddRange((permissions);
            claims.AddRange(userClaims);
            var userRoles = await _userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(_jwtOption.TokenLifetime);

            var token = new JwtSecurityToken(
              _jwtOption.Issuer,
              _jwtOption.Issuer,
              claims,
              expires: expires,
              signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result> ChangePasswordAsync(string passworrd, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(_auditService.UserId);
            if (user == null)
            {
                return Result.Failure(ApiExceptionType.NotFound, "User does not exist");
            }

            var result = await _userManager.ChangePasswordAsync(user, passworrd, newPassword);


            if (result.Succeeded)
            {
                return Result.Successed();
            }

            return Result.Failure(ApiExceptionType.InvalidLogin, "", result.Errors.Select(s => s.Description));
        }

        public async Task<Result> ForgetPasswordAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Result.Failure(ApiExceptionType.NotFound, "User does not exist");

            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $" auth / reset - password ? token = {token} +  & email =  + {user.Email}";


            var callback = _urlHelper.GetCurrentUrl(url);

            return Result.Successed();
        }

        public async Task<Result> ResetPasswordAsync(string token, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(ApiExceptionType.NotFound, "User does not exist");

            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
            if (resetPassResult.Succeeded)
            {
                return Result.Successed();
            }

            return Result.Failure(ApiExceptionType.InvalidLogin, "", resetPassResult.Errors.Select(s => s.Description));

        }
    }
}