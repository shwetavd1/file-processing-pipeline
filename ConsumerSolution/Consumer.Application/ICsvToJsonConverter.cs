using Consumer.Domain;

namespace Consumer.Application
{
	public interface ICsvToJsonConverter
	{
		Task<List<ProcessedData>> ConvertAsync(string csvData); //take csv data and converts into structured objects(json format)
	}
}