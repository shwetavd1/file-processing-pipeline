namespace Producer.Application.Services
{
    public interface IPollingService
    {
        Task WaitForNextCycle (CancellationToken cancellationToken);
    }
}
