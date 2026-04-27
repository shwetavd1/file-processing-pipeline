namespace Producer.Application.Interfaces
{
    public interface IConverterFactory
    {
        IFileConversion GetConverter(string fileName);
    }
}
