using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Fashe.Users.Producers.Interfaces;
using Fashe.Users.Producers.Producers;

namespace Fashe.Users.Producers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProducers(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddTransient<IUserProducer, UserProducer>();

            return services;
        }
    }
}
