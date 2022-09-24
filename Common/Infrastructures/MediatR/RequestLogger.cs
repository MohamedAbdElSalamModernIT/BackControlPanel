using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NJsonSchema.Infrastructure;

namespace Common.Infrastructures.MediatR {
  public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest> {
    private readonly ILogger _logger;

    private readonly IHttpContextAccessor _contextAccessor;

    public RequestLogger(ILogger<TRequest> logger,IHttpContextAccessor contextAccessor //, ILog log, IAuditService auditService
    )
    {
      _logger = logger;
      _contextAccessor = contextAccessor;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken) {
      var name = typeof(TRequest).Name;
      if (_contextAccessor.HttpContext.Request.Path.HasValue &&
          _contextAccessor.HttpContext.Request.Path.Value.Contains("add-promoter-survey"))
      {
        _logger.LogInformation("[Roi Request logger]: {Name} {@Request}", name, request);
      }
      var currentUser=_contextAccessor.HttpContext.User?.Identity?.Name??"";

      var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
      jsonResolver.IgnoreProperty(typeof(TRequest), "IdentityFace", "IdentityBack", "DriveLic", "Img", "PersonalImg");

      var serializerSettings = new JsonSerializerSettings {ContractResolver = jsonResolver};

      // TODO: Add User Details
      _logger.LogInformation("RazPocket Request: {Name} {@Request}", name, request);

      return Task.CompletedTask;
    }
  }
}
