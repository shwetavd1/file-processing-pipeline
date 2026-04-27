
using Consumer.Domain;

namespace Consumer.Application
{
	public interface IMessageConsumer<T>
	{
		//consumer can work with any type of message 
		event EventHandler<MessageData<T>> OnMessageReceived;// interface defines contract, Event should be part of contract so worker doesn't depend on concrete class
		Task ConsumeAsync();
	} 
}