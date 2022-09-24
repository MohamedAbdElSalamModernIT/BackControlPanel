using System.Collections.Generic;
using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Web.Extensions {
  public class SwaggerInstaller:IInstaller 
  {
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo() {
          Version = "v1",
          Title = "ToDo API",
          Description = "A simple example ASP.NET Core Web API",
          Contact = new OpenApiContact() {
            Name = "Shayne Boyer",
            Email = string.Empty,
          },
          License = new OpenApiLicense() {
            Name = "Use under LICX",
          }
        });
        c.AddSecurityDefinition("Bearer",
          new OpenApiSecurityScheme() {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
          });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
          {
            new OpenApiSecurityScheme()
            {
              Reference = new OpenApiReference()
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header
            },
            new List<string>()
          },
        });
      });
    }
  }
}
