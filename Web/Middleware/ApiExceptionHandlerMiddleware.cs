using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Web.Middleware {
  public class ApiExceptionHandlerMiddleware {
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly JsonSerializerSettings _jsonSettings;
    private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;

    public ApiExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHostEnvironment env) {
      _next = next ?? throw new ArgumentNullException(nameof(next));
      _env = env;
      _jsonSettings = ConfigureJsonSettings();
      _logger = loggerFactory?.CreateLogger<ApiExceptionHandlerMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    protected virtual JsonSerializerSettings ConfigureJsonSettings() {
      var serializerSettings = new JsonSerializerSettings();
      serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      return serializerSettings;
    }

    public async Task InvokeAsync(HttpContext context) {
      try {
        await _next(context);
      } catch(ApiException ex) {
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)ex.StatusCode;
        var json = JsonConvert.SerializeObject(ex, _jsonSettings);
        await context.Response.WriteAsync(json);

        _logger.LogError(ex.ErrorMessage,json);
        return;
      } catch(Exception ex) {
        context.Response.Clear();
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = context.Request.ContentType;
        if (_env.IsDevelopment()) {
          var json = JsonConvert.SerializeObject(ex, _jsonSettings);
          await context.Response.WriteAsync(json);
        } else {
          var json = JsonConvert.SerializeObject(ex.Message, _jsonSettings);
          await context.Response.WriteAsync(json);
        }

        _logger.LogError(ex, "Api error");
        return;
      }
    }
  }
}