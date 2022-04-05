using MediatR;
using Fashe.Users.Core.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Helpers.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheableMediatorQuery
    {
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> logger;

        private readonly IDistributedCache cache;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger) =>
            (this.cache, this.logger) = (cache, logger);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            string cacheData = await cache.GetStringAsync(request.CacheKey);

            if (cacheData != null)
            {
                response = JsonSerializer.Deserialize<TResponse>(cacheData);

                logger.LogInformation($"Fetched from Cache -> \"{request.CacheKey}\"");

                return response;
            }

            response = await next();

            try
            {
                string jsonResponse = JsonSerializer.Serialize(response);

                await cache.SetStringAsync(request.CacheKey, jsonResponse);

                logger.LogInformation($"Added to Cache -> \"{request.CacheKey}\"");
            }
            catch
            {
                logger.LogWarning($"Failed to add to cache -> \"{request.CacheKey}\"");
            }

            return response;
        }
    }
}
