using Microsoft.Extensions.DependencyInjection;
using NewsWeb.Infrastructure.Services.Mail;
using NewsWeb.Infrastructure.Services.RabbitMQ;

namespace NewsWeb.Infrastructure.Extensions
{
    public static class InfrastructureDependncyConfig
    {
        /// <summary>
        /// Infrastructure katmanında kullanılan dependency ler
        /// </summary>
        public static IServiceCollection AddInfrastructureDependecyConfig(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, MailService>();

            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddHostedService<ConsumerHostedService>();

            return services;
        }
    }
}
