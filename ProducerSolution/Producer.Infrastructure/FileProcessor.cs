/* read xml content
 * parse using xml parser
 * validate data
 */

using Producer.Application;
using Producer.Domain;

namespace Producer.Infrastructure
{
    public class FileProcessor : IFileProcessor
    {
        public async Task<FileData> ProcessAsync(FileData file)
        {
            // check file has content or not if yes then only return
            if (string.IsNullOrWhiteSpace(file.Content))
                throw new Exception("File content is empty");

            Console.WriteLine($"Processing File : {file.FileName}");

            // returns completed task with result
            return await Task.FromResult(file);
        }
    }
}

