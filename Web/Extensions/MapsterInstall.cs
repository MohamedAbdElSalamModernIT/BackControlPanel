using System.Reflection;
using Application.Category.Commands;
using Common.Interfaces;
using Infrastructure;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Web.Extensions.Services {
  public class MapsterInstall : IInstaller{
    public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration) {
      var config = TypeAdapterConfig.GlobalSettings;
      // config.EnableJsonMapping();
      config.Scan(new Assembly[] {
        (typeof(Startup)).Assembly,
        (typeof(CreateCategoryCommand)).Assembly,
      });
    }
  }
}