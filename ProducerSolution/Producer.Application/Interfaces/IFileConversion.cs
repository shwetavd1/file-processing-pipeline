namespace Producer.Application.Interfaces
{
    public interface IFileConversion
    {
        // allows each strategy to declare whether it supports a file type
        // enables dynamic selection
        bool canHandle(string fileExtension);
        Task<string> ConvertAsync(Stream content);
    }
}
