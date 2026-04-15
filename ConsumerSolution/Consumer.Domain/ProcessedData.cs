using System;
namespace Consumer.Domain
{
	public class ProcessedData
	{
		public int Id { get; init; }
		public string Name { get; init; } = string.Empty;//Initialized with string.Empty so it’s never null. Prevents NullReferenceException.
		public int Age { get; set; }
	}
}
