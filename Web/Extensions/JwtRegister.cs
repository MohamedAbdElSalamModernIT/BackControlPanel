using System;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Web.Globals;

namespace Web.Extensions
{
  public class JwtRegister : IInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
      var jwtOption = configuration?.GetSection(nameof(Sections.JwtOption))?.Get<JwtOption>();
      var tokenValidateParameters=new TokenValidationParameters {
        ValidIssuer = jwtOption.Issuer,
        ValidAudience = jwtOption.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Key)),
        ClockSkew = TimeSpan.Zero // remove delay of token when expire
      };

      services.AddSingleton<TokenValidationParameters>(tokenValidateParameters);
      services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(cfg => {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = tokenValidateParameters;
        cfg.Authority = "";
        cfg.Events = new JwtBearerEvents {
          OnMessageReceived = context => {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/notificationHub"))) {
              // Read the token out of the query string
              context.Token = accessToken;
            }

            return Task.CompletedTask;
          }
        };

      });

    }
  }
}