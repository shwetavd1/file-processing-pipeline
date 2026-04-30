namespace Producer.Application.Configurations
{
    public class RabbitMqSettings
    {
        public string? HostName { get; set; }
        public string? QueueName { get; set; }
        public string? ExchangeName { get; set; }
        public string? RoutingKey { get; set; }

        public int MessageTtlMs { get; set; }
        public int MaxQueueLength { get; set; }

        public DeadLetterSettings? DeadLetter { get; set; }
    }
}