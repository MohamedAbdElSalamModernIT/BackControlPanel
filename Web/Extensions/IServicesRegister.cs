using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions {
    public interface IServicesRegister {
        void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}