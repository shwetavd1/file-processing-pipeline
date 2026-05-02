using Producer.Domain.Entities;
namespace Producer.Application.Processing
{
    public interface IFileFetcher
    {
        Task<IEnumerable<IFileData>> GetFilesAsync(string folderPath);
    }
}
