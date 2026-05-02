using Producer.Application.Conversion;
using Producer.Application.Messaging;
using Producer.Domain.Entities;

namespace Producer.Infrastructure.Processing
{
    public class FileValidator : IFileValidator
    {
        private readonly IConverterFactory _converterFactory;
        public FileValidator(IConverterFactory converterFactory) => _converterFactory = converterFactory;
        public IEnumerable<IFileData> GetValidFiles(IEnumerable<IFileData> files)
        {
            foreach (var file in files)
            {
                try
                {
                    _converterFactory.GetConverter(file.FileName);
                }
                catch (NotSupportedException)
                {
                    continue;
                }
                yield return file;
            }
        }
    }
}
