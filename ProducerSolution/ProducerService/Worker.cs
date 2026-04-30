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
            _logger.LogInformation("Producer Worker started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Starting producer polling cycle.");

                    await _orchestrator.RunOnceAsync(stoppingToken);

                    _logger.LogInformation("Producer polling cycle completed. Waiting for next run.");

                    await _pollingService.WaitForNextCycle(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Producer Worker cancellation requested.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex,"Producer Worker terminated due to an unhandled exception.");
                throw;
            }
            finally
            {
                _logger.LogInformation("Producer Worker stopped.");
            }
        }
    }
}