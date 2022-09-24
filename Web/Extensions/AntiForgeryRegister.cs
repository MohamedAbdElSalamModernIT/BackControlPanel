using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions {
  public class AntiForgeryRegister : IInstaller {
    public void InstallService(IServiceCollection services, IConfiguration configuration) {
      // services.AddAntiforgery(options => {
      //   options.HeaderName = "X-XSRF-TOKEN";
      // });
    }
  }
}