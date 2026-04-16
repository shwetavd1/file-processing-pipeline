
namespace Producer.Application
{
    public interface IMessagePublisher
    {
        void Publish(string message);
    }
}
