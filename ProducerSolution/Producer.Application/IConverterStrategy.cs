/* converts xml to csv
 */

namespace Producer.Application
{
    public interface IConverterStrategy
    {
        Task<string> ConvertAsync(string xmlContent);
    }
}
