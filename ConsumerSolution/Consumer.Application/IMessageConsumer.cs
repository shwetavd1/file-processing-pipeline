using System;
using Consumer.Domain;

namespace Consumer.Application

public interface IMessageConsumer
{
	Task<MessageData> ConsumeAsync(); 
} //reads message from queue
