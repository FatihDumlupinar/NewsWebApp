using Microsoft.Extensions.Options;
using NewsWeb.Models.RabbitMQ;
using RabbitMQ.Client;

namespace NewsWeb.Infrastructure.Services.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMQSettings _options;

        public RabbitMqService(IOptions<RabbitMQSettings> options)
        {
            _options = options.Value;
        }

        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                Uri = new(_options.Url)
            };

            connection.DispatchConsumersAsync = true;

            var channel = connection.CreateConnection();

            return channel;
        }
    }
}
