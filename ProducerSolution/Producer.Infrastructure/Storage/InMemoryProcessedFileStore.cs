using Producer.Application.Processing;
using System.Collections.Concurrent;

namespace Producer.Infrastructure.Storage
{
    public class InMemoryProcessedFileStore : IProcessedFileStore 
    {
        private readonly ConcurrentDictionary<string,bool> _processedFiles = new();
        public bool IsProcessed(string fileName)
        {
            return _processedFiles.ContainsKey(fileName);
        }

        public void MarkAsProcessed(string fileName)
        {
            _processedFiles[fileName] = true;
        }
    }
}
