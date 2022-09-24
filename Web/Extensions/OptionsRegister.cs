using Boxed.AspNetCore;
using Common.Interfaces;
using Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Globals;

namespace Web.Extensions
{
  public class OptionsRegister : IInstaller {
    public void InstallService(IServiceCollection services, IConfiguration configuration) {    

      services
        .ConfigureAndValidateSingleton<JwtOption>(configuration.GetSection(nameof(Sections.JwtOption)))
        .ConfigureAndValidateSingleton<AppInfoOption>(configuration.GetSection(nameof(Sections.AppInfoOption)))
        .ConfigureAndValidateSingleton<ImageOption>(configuration.GetSection(nameof(Sections.ImageOption)))
        ;
    }
  }
}