using Producer.Application.Interfaces;
using Producer.Domain;

namespace Producer.Infrastructure
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
            return await converter.ConvertAsync(file.Content);
        }
    }
}

