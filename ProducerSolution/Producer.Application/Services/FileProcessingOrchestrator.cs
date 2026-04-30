using Microsoft.Extensions.Options;
using Producer.Application.Configurations;
using Producer.Application.Messaging;
using Producer.Application.Processing;
using Producer.Application.Services;

public class FileProcessingOrchestrator : IFileProcessingOrchestrator
{
    private readonly IFileFetcher _fileFetcher;
    private readonly IFileValidator _fileValidator;
    private readonly IFilePipelineExecutor _executor;
    private readonly FileSettings _settings;
    private readonly ILogger<FileProcessingOrchestrator> _logger;
    private readonly IProcessedFileStore _processedFileStore;

    public FileProcessingOrchestrator(
        IFileFetcher fileFetcher,
        IFileValidator fileValidator,
        IFilePipelineExecutor executor,
        IOptions<FileSettings> options,
        ILogger<FileProcessingOrchestrator>logger,
        IProcessedFileStore processedFileStore)
        => (_fileFetcher, _fileValidator, _executor, _settings, _logger, _processedFileStore)
        = (fileFetcher, fileValidator, executor, options.Value, logger, processedFileStore);
    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting file processing");

        var files = await _fileFetcher.GetFilesAsync(_settings.RootFolderPath);

        var validFiles = _fileValidator.GetValidFiles(files);

        var processedFiles = validFiles.Where(f => !_processedFileStore.IsProcessed(f.FileName)).ToList();

        await _executor.ExecuteAsync(processedFiles, cancellationToken);

        _logger.LogInformation("Completed file processing");
    }
}