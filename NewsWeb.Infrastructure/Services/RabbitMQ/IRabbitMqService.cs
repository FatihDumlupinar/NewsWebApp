using RabbitMQ.Client;

namespace NewsWeb.Infrastructure.Services.RabbitMQ
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}
