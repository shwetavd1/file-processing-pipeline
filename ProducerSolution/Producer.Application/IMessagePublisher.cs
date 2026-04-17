// to publish the message to queue
namespace Producer.Application
{
    public interface IMessagePublisher
    {
        void Publish(string message);
    }
}
