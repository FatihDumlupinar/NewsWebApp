using Microsoft.Extensions.DependencyInjection;
using NewsWeb.Infrastructure.Services.Mail;
using NewsWeb.Models.Mail;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NewsWeb.Infrastructure.Services.RabbitMQ
{
    public class ConsumerService : IConsumerService, IDisposable
    {
        #region Ctor&Fields

        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly IMailService _mailService;
        private const string _queueName = "User";

        public ConsumerService(IRabbitMqService rabbitMqService, IMailService mailService)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("UserExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "UserExchange", string.Empty);
            _mailService = mailService;
        }

        #endregion

        public async Task ReadQueue(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var request = JsonConvert.DeserializeObject<MailRequest>(content);

                await _mailService.SendEmailAsync(request);

                await Task.CompletedTask;

                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
