using Producer.Application.Processing;
using Producer.Domain.Entities;
namespace Producer.Infrastructure.Storage
{
    public class FileFetcher : IFileFetcher
    {
        public async Task<IEnumerable<IFileData>> GetFilesAsync(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException($"Folder path cannot be null or empty: {folderPath}");

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            var files = new List<IFileData>();

            var paths = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (var path in paths)
            {
                try
                {
                    var fileData = new FileData(Path.GetFileName(path), path, null);
                    files.Add(fileData);
                }
                catch
                {
                    continue;
                }
            }
            await Task.CompletedTask;
            return files;
        }
    }
}
