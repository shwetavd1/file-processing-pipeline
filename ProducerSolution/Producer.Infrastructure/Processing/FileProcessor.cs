using Producer.Application.Conversion;
using Producer.Application.Messaging;
using Producer.Domain.Entities;

namespace Producer.Infrastructure.Processing
{
    public class FileProcessor : IFileProcessor<FileData, string>
    {
        private readonly IConverterFactory _factory;

        public FileProcessor(IConverterFactory factory)
        {
            _factory = factory;
        }
        public async Task<string> ProcessAsync(FileData file)
        {
           if( file.Content == null)
                throw new InvalidOperationException("File content is empty");

            var converter = _factory.GetConverter(file.FileName);
            using var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
            return await converter.ConvertAsync(file.Content);
        }
    }
}

