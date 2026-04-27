using Producer.Application.Interfaces;
using Producer.Domain;
using System.Security.Cryptography;
namespace Producer.Infrastructure
{
    public class FileFetcher : IFileFetcher
    {
        public async Task<IEnumerable<IFileData>> GetFilesAsync(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException($"Folder path cannot be null, empty, or whitespace.{folderPath}");

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The folder path does not exist: {folderPath}");

            var files = new List<IFileData>();

            var paths = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (var path in paths)
            {
                try
                {
                    var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                    var fileData = new FileData(Path.GetFileName(path), path, stream);
                    files.Add(fileData);
                }
                catch(Exception)
                {
                    continue;
                }
                
            }
            await Task.CompletedTask;
            return files;
        }
    }
}
