using System.Reflection;
using Application.Auth.Commands;
using Common.Infrastructures.MediatR;
using Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructures;

namespace Web.Extensions {
  public class MediatRInstaller : IInstaller {
    public void InstallService(IServiceCollection services, IConfiguration configuration) {
      services.AddMediatR(new[] {
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(LoginCommand)),
      });
      //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipeline<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
  }
}