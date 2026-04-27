using Producer.Application.Services;

namespace ProducerService
{
    public class Worker : BackgroundService
    {
        private readonly IFileProcessingOrchestrator _orchestrator;
        private readonly IPollingService _pollingService;

        public Worker(
            IFileProcessingOrchestrator orchestrator,
            IPollingService pollingService)
        {
            _orchestrator = orchestrator;
            _pollingService = pollingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _orchestrator.RunOnceAsync(stoppingToken);
                await _pollingService.WaitForNextCycle(stoppingToken);
            }
        }
    }
}