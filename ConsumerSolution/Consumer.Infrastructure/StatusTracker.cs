using Consumer.Application;

namespace Consumer.Infrastructure 
{
	public class StatusTracker : IStatusTracker
	{
		private readonly Dictionary<int, string> _statusStore = new(); 
		public StatusTracker()
		{
			_statusStore = new Dictionary<int, string>();
		}
		public void MarkAsPending(int messageId)
		{
			_statusStore[messageId] = "Pending"; 
			Console.WriteLine($"Message {messageId} is Pending");
		}
		public void MarkAsCompleted(int messageId)
		{
			_statusStore[messageId] = "Completed";
			Console.WriteLine($"Message {messageId} is Completed");
		}
		public void MarkAsFailed(int messageId, string reason)
		{
			_statusStore[messageId] = "Failed: {reason}";
			Console.WriteLine($"Message {messageId} is Failed: {reason}");
		}

	}
}
