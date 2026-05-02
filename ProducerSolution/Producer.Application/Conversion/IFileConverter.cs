namespace Producer.Application.Conversion
{
    public interface IFileConverter
    {
        bool canHandle(string fileExtension);
        Task<string> ConvertAsync(Stream content);
    }
}
