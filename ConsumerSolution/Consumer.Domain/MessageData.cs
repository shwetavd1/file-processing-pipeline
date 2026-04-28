using System;
namespace Consumer.Domain
{
	public class MessageData<T>
	{
		public int Id { get; init; }
		public T Content { get; set; } = default! ; // message can carry any type instead of just string
		public DateTime ReceivedTime { get; init; } = DateTime.UtcNow; // UTC is used to avoid timezone issues, especially in distributed systems.
	}
}

