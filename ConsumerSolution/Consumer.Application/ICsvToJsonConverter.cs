using Consumer.Domain;

namespace Consumer.Application
{
	public interface ICsvToJsonConverter<T>
	{
        //Task<List<ProcessedData>> ConvertAsync(string csvData); this is only works for processed data but we can make it more generic by using T as a type parameter
        Task<List<Dictionary<string,object>>> ConvertAsync(string input); 
    }
}