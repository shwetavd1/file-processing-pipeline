using System;
namespace Consumer.Application
{
	public interface IStatusTracker
	{
		void MarkAsPending(int messageId);
		void MarkAsCompleted(int messageId);
		void MarkAsFailed(int messageId, string reason);
	}
}
