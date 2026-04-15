using Consumer.Application;

namespace Consumer.Infrastructure //Infrasturucture layer contains actual implementation
{
	//we need to do this status tracker because many messages will come, some will succeed some will fail so we must have to track each message status.
	public class StatusTracker : IStatusTracker
	{
		private readonly Dictionary<int, string> _statusStore = new(); // used dictionary for Key value (messageId, status), Fast lookup, Easy Update
		public StatusTracker()
		{
			_statusStore = new Dictionary<int, string>();
		}
		public void MarkAsPending(int messageId)
		{
			_statusStore[messageId] = "Pending"; //store status
			Console.WriteLine($"Message {messageId} is Pending");
		}
		public void MarkAsCompleted(int messageId)
		{
			_statusStore[messageId] = "Completed";
			Console.WriteLine($"Message {messageId} is Completed");
		}
		public void MarkAsFailed(int messageId, string reason)
		{
			_statusStore[messageId] = "Failed: {reason}";// stores status as well as reason
			Console.WriteLine($"Message {messageId} is Failed: {reason}");
		}

	}
}

/*
 Alertantive approaches are here 
 easy [data loss when app stopes] currently used
alternative 1: DataBase - messagId, status stored in DB but it needs setup
alternative 2: File storage- simple persistence but it is slower
alternative 3: Distributed cache- used in real systems but it is complex
 */

// Now the flow is like: message comes->mark pending-> convert csv-JSON -> Process->Mark complete/failed. 