using System;
namespace Consumer.Domain
{
	public class MessageData //MessageData is a plain object used to hold information about a message that contains CSV content and the time it was received.
	{
		public int Id { get; init; }
		public string CsvContent { get; init; } = string.Empty; //Initialized with string.Empty so it’s never null. Prevents NullReferenceException.
		public DateTime ReceivedTime { get; init; } = DateTime.UtcNow;//UTC is used to avoid timezone issues, especially in distributed systems.
	}
}

