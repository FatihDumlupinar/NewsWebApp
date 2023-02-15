namespace NewsWeb.Infrastructure.Services.RabbitMQ
{
    public interface IConsumerService
    {
        Task ReadQueue(CancellationToken cancellationToken);
    }
}
