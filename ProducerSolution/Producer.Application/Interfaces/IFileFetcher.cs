using Producer.Domain;
namespace Producer.Application.Interfaces
{
    public interface IFileFetcher
    {
        Task<IEnumerable<IFileData>> GetFilesAsync(string folderPath);
    }
}
