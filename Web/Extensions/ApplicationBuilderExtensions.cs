using Microsoft.AspNetCore.Builder;
using Web.Middleware;

namespace Web.Extensions {
  public static partial class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder application) {
      return application.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
  }
}