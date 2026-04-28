using Consumer.Domain;

namespace Consumer.Application
{
	public interface ICsvToJsonConverter<T>
	{
        Task<List<Dictionary<string,object>>> ConvertAsync(string input); 
    }
}