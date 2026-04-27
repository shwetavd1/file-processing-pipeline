namespace Producer.Application.Interfaces
{
    public interface IMessagePublisher
    {
        public Task Publish(string message, string fileName);
    }
}
