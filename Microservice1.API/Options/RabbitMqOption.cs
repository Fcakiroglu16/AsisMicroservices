namespace Microservice1.API.Options
{
    public class RabbitMqOption
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
