namespace Producer.Application.Services
{
    public interface IFileProcessingOrchestrator
    {
        Task RunOnceAsync(CancellationToken cancellationToken);
    }
}
