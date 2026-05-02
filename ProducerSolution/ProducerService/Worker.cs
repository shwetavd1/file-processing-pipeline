using Producer.Application.Services;

namespace ProducerService
{
    public class Worker : BackgroundService
    {
        private readonly IFileProcessingOrchestrator _orchestrator;
        private readonly IPollingService _pollingService;
        private readonly ILogger<Worker> _logger;
        public Worker(
            IFileProcessingOrchestrator orchestrator,
            IPollingService pollingService,
            ILogger<Worker> logger)
            => (_orchestrator, _pollingService, _logger)
            = (orchestrator, pollingService, logger);
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