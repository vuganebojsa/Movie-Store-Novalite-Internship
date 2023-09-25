using MediatR;
using System.Text.Json;

namespace MovieStore.Api.LoggingHandlers
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            _logger.LogInformation($"[START] {requestName}");
            TResponse response;

            _logger.LogInformation($"[PROPS] {requestName} {JsonSerializer.Serialize(request)}");

            response = await next();

            _logger.LogInformation(
                $"[END] {requestName}");

            return response;
        }
    }
}
