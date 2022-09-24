using System.Collections.Generic;
using System.Reflection;
using Application.Validators;
using Common.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions {
  public class FluentValidationRegister : IInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; })
        .AddMvc()
        .AddFluentValidation(confg => {
          confg.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
          confg.RegisterValidatorsFromAssemblies(new List<Assembly>() {
            typeof(LoginValidator).Assembly
          });
        });
    }
  }
}