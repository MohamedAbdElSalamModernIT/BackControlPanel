using System.Reflection;
using AutoMapper;
using Common.Interfaces;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Web.Profiler;

namespace Web.Extensions {

    public class ServicesRegister : IInstaller {
        public void InstallService(IServiceCollection services, IConfiguration configuration) {
            services
              .AddAutoMapper(new Assembly[] {
          typeof(AutoMapperProfile).GetTypeInfo().Assembly,
              })
              .AddHttpContextAccessor()
              .AddHttpClient()
              .AddScoped<IPermissionService, PermissionService>()
              .AddScoped<IAppDbContext>(s => s.GetService<AppDbContext>())
              .AddScoped<IAuditService, AuditService>()
              .AddScoped<IIdentityService, IdentityService>()
              .AddScoped<IImageService, ImageService>()
              .AddScoped<IUrlHelper, UrlHelper>()
              .AddScoped<IXmlService, XmlService>()
              .AddScoped<IExcelService, ExcelService>()
              .AddSingleton<ISettingService, SettingService>();

            ;

        }
    }
}
