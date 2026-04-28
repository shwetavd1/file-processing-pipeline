using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Producer.Application.Interfaces;
using Producer.Application.Services;
using Producer.Domain;

public class FileProcessingOrchestrator : IFileProcessingOrchestrator
{
    private readonly IFileFetcher _fileFetcher;
    private readonly IFileProcessor<FileData, string> _fileProcessor;
    private readonly IMessagePublisher _publisher;
    private readonly FileSettings _settings;
    private readonly ILogger<FileProcessingOrchestrator> _logger;

    public FileProcessingOrchestrator(
        IFileFetcher fileFetcher,
        IFileProcessor<FileData, string> fileProcessor,
        IMessagePublisher publisher,
        IOptions<FileSettings> options,
        ILogger<FileProcessingOrchestrator> logger)
    {
        _fileFetcher = fileFetcher;
        _fileProcessor = fileProcessor;
        _publisher = publisher;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting file processing");
        var files = await _fileFetcher.GetFilesAsync(_settings.RootFolderPath);

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogInformation("Processing file {fileName}", file.FileName);
                var result = await _fileProcessor
                    .ProcessAsync((FileData)file);

                await _publisher.Publish(
                    result,
                    ((FileData)file).FileName);
            }
            catch (NotSupportedException)
            {
                _logger.LogWarning("Skipping unsupported file {fileName}", file.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process file {fileName}", file.FileName);
            }
            finally
            {
                if (file is FileData fd)
                    fd.Content?.Dispose();
            }
        }
        _logger.LogInformation("Completed file processing");
    }
}