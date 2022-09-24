using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Infrastructures.MediatR
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        private readonly IOptions<AppInfoOption> _infoOptions;
        //private readonly ILog _log;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger,IOptions<AppInfoOption> infoOptions//, ILog log
        )
        {
            _timer = new Stopwatch();
            _logger = logger;
            _infoOptions = infoOptions;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)
            {
                var name = typeof(TRequest).Name;

                // TODO: Add User Details
                //_log.Warning($"HelpApp Long Running Request: {name} ({_timer.ElapsedMilliseconds} milliseconds) {request}");

                _logger.LogWarning("{AppName} Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", _infoOptions.Value.Name,name, _timer.ElapsedMilliseconds, request);
            }

            return response;
        }
    }
}
