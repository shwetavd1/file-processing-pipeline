using Microsoft.Extensions.Logging;
using Producer.Application.Interfaces;
using Producer.Application.Messaging;
using Producer.Application.Processing;
using Producer.Domain.Entities;

namespace Producer.Infrastructure.Processing
{
    public class FilePipelineExecutor : IFilePipelineExecutor
    {
        private readonly IFileProcessor<FileData, string> _fileProcessor;
        private readonly IMessagePublisher _publisher;
        private readonly ILogger<FilePipelineExecutor> _logger;
        private readonly IProcessedFileStore _processedFileStore;
        public FilePipelineExecutor(
            IFileProcessor<FileData, string> fileProcessor, IMessagePublisher publisher,
            ILogger<FilePipelineExecutor> logger, IProcessedFileStore processedFileStore) 
            => (_fileProcessor, _publisher, _logger, _processedFileStore) 
            = (fileProcessor, publisher, logger, processedFileStore);

        public async Task ExecuteAsync(IEnumerable<IFileData> files, CancellationToken cancellationToken)
        {
            var fileList = files.ToList();
            int totalFiles = fileList.Count;
            int successCount = 0;
            int failedCount = 0;
            var semaphore = new SemaphoreSlim(5);

            var tasks = fileList.Select(async file =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    _logger.LogInformation("Start Processing {fileName}", file.FileName);

                    var result = await _fileProcessor.ProcessAsync((FileData)file);

                    await Task.Delay(2000);

                    await _publisher.Publish(result, file.FileName);

                    _processedFileStore.MarkAsProcessed(file.FileName);

                    Interlocked.Increment(ref successCount);

                    _logger.LogInformation("End Processing {fileName}", file.FileName);
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref failedCount);
                    _logger.LogError(ex, "Error processing {fileName}", file.FileName);
                }
                finally
                {
                    if (file is FileData fd)
                        fd.Content?.Dispose();
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
            _logger.LogInformation("Processing Summary: Total={total}, Success={success}, Failed={failed}",totalFiles,successCount,failedCount);
        }
    }
}