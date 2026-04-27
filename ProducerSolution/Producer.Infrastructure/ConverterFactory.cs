using Producer.Application.Interfaces;
namespace Producer.Infrastructure
{
    public class ConverterFactory : IConverterFactory
    {
        private readonly IEnumerable<IFileConversion> _converters;

        public ConverterFactory(IEnumerable<IFileConversion> converters)
        {
            _converters = converters;
        }
        public IFileConversion GetConverter(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            var converter = _converters.FirstOrDefault(c =>
            c.canHandle(extension));

            if (converter == null)
                throw new NotSupportedException($"No converter found {extension}");

            return converter;
        }
    }
}
