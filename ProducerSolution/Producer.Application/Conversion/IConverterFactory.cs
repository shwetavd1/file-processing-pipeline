namespace Producer.Application.Conversion
{
    public interface IConverterFactory
    {
        IFileConverter GetConverter(string fileName);
    }
}
