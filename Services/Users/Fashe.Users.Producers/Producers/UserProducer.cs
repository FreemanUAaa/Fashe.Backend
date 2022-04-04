using Fashe.Users.Producers.Contracts;
using Fashe.Users.Producers.Interfaces;
using MassTransit;
using Messages.Users.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fashe.Users.Producers.Producers
{
    public class UserProducer : IUserProducer
    {
        private readonly ISendEndpointProvider endpointProvider;

        private readonly ILogger<UserProducer> logger;

        public UserProducer(ISendEndpointProvider endpointProvider, ILogger<UserProducer> logger) =>
            (this.endpointProvider, this.logger) = (endpointProvider, logger);

        public async Task UserCreated(Guid userId)
        {
            ISendEndpoint endpoint = await endpointProvider.GetSendEndpoint(new Uri(RabbitMQContracts.UserCreated));

            await endpoint.Send<IUserCreated>(new { UserId = userId });

            logger.LogInformation($"Successful message added to queue -> {RabbitMQContracts.UserCreated}");
        }

        public async Task UserDeleted(Guid userId)
        {
            ISendEndpoint endpoint = await endpointProvider.GetSendEndpoint(new Uri(RabbitMQContracts.UserDeleted));

            await endpoint.Send<IUserDeleted>(new { UserId = userId });

            logger.LogInformation($"Successful message added to queue -> {RabbitMQContracts.UserDeleted}");
        }
    }
}
