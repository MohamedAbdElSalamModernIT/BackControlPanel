using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions {
  public class JsonRegister: IInstaller {
    public void InstallService(IServiceCollection services, IConfiguration configuration) {
      // services.AddMvc(options => options.EnableEndpointRouting = false)
      //   .AddJsonOptions(options =>
      //     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
    }
  }
}