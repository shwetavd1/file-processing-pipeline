using System;
using Consumer.Domain;

namespace Consumer.Application
{
	public interface IMessageConsumer
	{
		event Action<MessageData> OnMessageReceived;// interface defines contract, Event should be part of contract so worker doesn't depend on concrete class
		Task<MessageData> ConsumeAsync(); // used interface for loose coupling, can be replace inplementation easily
	} //reads message from queue
}