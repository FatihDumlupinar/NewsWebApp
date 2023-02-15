namespace NewsWeb.Models.RabbitMQ
{
    public class RabbitMQSettings
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string HostName { get; set; } = string.Empty;

        public int Port { get; set; } = 0;

        public string VHost { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
