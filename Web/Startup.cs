using Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using AutoMapper;
using Common.Extensions;
using Web.Extensions;
using Web.Middleware;
using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors();

            services.AddControllers()
                      .AddJsonOptions(options =>
                       options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .AddNewtonsoftJson(opt => {
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    });
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                ;

            var installers = typeof(Startup).Assembly.ExportedTypes.Where(a =>
                typeof(IInstaller).IsAssignableFrom(a) && !a.IsInterface && !a.IsAbstract)
              .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();


            installers.ForEach(i => i.InstallService(services, Configuration));

            if (services != null) {
                var provider = services.BuildServiceProvider();

                BaseEntityExtension.Configure(provider.GetService<IMapper>());
                provider.GetRequiredService<IServiceScopeFactory>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiForgery) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseStaticFiles();

           

            app.UseAuthorization().UseCustomExceptionHandler()
              .UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                  c.RoutePrefix = "api";
              });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}