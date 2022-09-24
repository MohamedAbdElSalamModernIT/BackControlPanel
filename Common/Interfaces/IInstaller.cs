using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Interfaces {
  public interface IInstaller {
    void InstallService(IServiceCollection services, IConfiguration configuration);
  }
}