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

    public FileProcessingOrchestrator(
        IFileFetcher fileFetcher,
        IFileProcessor<FileData, string> fileProcessor,
        IMessagePublisher publisher,
        IOptions<FileSettings> options)
    {
        _fileFetcher = fileFetcher;
        _fileProcessor = fileProcessor;
        _publisher = publisher;
        _settings = options.Value;
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        var files = await _fileFetcher.GetFilesAsync(_settings.RootFolderPath);

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var result = await _fileProcessor
                    .ProcessAsync((FileData)file);

                await _publisher.Publish(
                    result,
                    ((FileData)file).FileName);
            }
            catch (NotSupportedException)
            {
                // Unsupported file type – skip safely
            }
            catch (Exception)
            {
                // Log and continue (logging can be added later)
            }
            finally
            {
                if (file is FileData fd)
                    fd.Content?.Dispose();
            }
        }
    }
}