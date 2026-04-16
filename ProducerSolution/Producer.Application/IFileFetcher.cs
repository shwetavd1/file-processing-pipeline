//reads xml from folder, retuns list of files
using Producer.Domain;
namespace Producer.Application
{
    public interface IFileFetcher
    {
        /* returns list of files
         * task - allows async operation, used for non-blocking operations
         */
        Task<List<FileData>> GetFilesAsync(string folderPath);
    }
}
