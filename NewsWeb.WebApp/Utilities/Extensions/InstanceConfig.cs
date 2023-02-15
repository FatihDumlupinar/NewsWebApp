using NewsWeb.Models.Mail;
using NewsWeb.Models.RabbitMQ;

namespace NewsWeb.WebApp.Utilities.Extensions
{
    public static class InstanceConfig
    {
        public static IServiceCollection AddInstanceConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("EmailConfiguration"));

            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQConfiguration"));

            return services;
        }
    }
}
