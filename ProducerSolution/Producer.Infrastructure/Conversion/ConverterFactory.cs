using Producer.Application.Conversion;
namespace Producer.Infrastructure.Conversion
{
    public class ConverterFactory : IConverterFactory
    {
        private readonly IEnumerable<IFileConverter> _converters;

        public ConverterFactory(IEnumerable<IFileConverter> converters)
        {
            _converters = converters;
        }
        public IFileConverter GetConverter(string fileName)
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
