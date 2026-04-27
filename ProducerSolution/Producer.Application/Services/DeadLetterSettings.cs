namespace Producer.Application.Services
{
    public class DeadLetterSettings
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RoutingKey { get; set; }
    }
}
