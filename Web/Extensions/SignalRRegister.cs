using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions {
    public class SignalRRegister : IInstaller {
        public void InstallService(IServiceCollection services, IConfiguration configuration) {
            services.AddSignalR();
        }
    }
}
