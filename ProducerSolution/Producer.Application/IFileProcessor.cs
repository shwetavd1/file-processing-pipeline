//to parse xml, validate, send conversion
using Producer.Domain;

namespace Producer.Application
{
    public interface IFileProcessor
    {
        Task<FileData> ProcessAsync(FileData file);
    }
}
