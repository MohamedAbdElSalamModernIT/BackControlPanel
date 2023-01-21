using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;

namespace Web {
    public class Program {
        public static async Task Main(string[] args) {
            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(config)
              .Enrich.FromLogContext()
              .Enrich.WithProperty("ApplicationName", "Baladia App")
              .Enrich.WithProcessId()
              //.Enrich.WithEnvironmentUserName()
              .CreateLogger();
            try {
                Log.Information("Application Starting.");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope()) {
                    var context = scope.ServiceProvider.GetService<AppDbContext>();

                    var concreteContext = context;
                    await concreteContext.Database.MigrateAsync();
                    await AppDbInitializer.Initialize(concreteContext, scope, "");
                }

                await host.RunAsync();
            } catch (Exception ex) {
                Log.Fatal(ex, "The Application failed to start.");
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
            .UseSerilog()
            // .UseSerilog((context, configuration) =>
            // {
            //   IHostEnvironment env = context.HostingEnvironment;
            //   string logsPath = env.ContentRootPath;
            //   configuration.Enrich.FromLogContext()
            //     .Enrich.WithProcessId()
            //     .Enrich.WithProcessName()
            //     .WriteTo.Console()
            //     .WriteTo.File(Path.Combine(logsPath, "logs", "service_.log"), rollingInterval: RollingInterval.Day)
            //     .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            //     .ReadFrom.Configuration(context.Configuration);
            // })
            .ConfigureWebHostDefaults(webBuilder => {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                webBuilder
              .UseWebRoot(currentDirectory + "/wwwroot")
              .UseStartup<Startup>();
            });

    }
}