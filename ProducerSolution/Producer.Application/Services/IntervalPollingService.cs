namespace Producer.Application.Services
{
    public class IntervalPollingService : IPollingService
    {
        private readonly TimeSpan _interval;

        public IntervalPollingService(TimeSpan interval)
        {
            _interval = interval;
        }

        public Task WaitForNextCycle(CancellationToken cancellationToken)
        {
            return Task.Delay(_interval, cancellationToken);
        }
    }
}
