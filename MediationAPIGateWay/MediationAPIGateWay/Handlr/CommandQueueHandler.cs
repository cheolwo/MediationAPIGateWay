using MediatR;

namespace MediationAPIGateWay.Handlr
{
    public class CommandQueueHandler<TCommand> : CommandBaseHandler<TCommand> where TCommand : IRequest<Unit>
    {
        private readonly ILogger<CommandQueueHandler<TCommand>> _logger;
        private readonly IQueueService _queueService;

        public CommandQueueHandler(ILogger<CommandQueueHandler<TCommand>> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        protected override async Task HandleCommandAsync(TCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing command: {command}");

            // 실제 비즈니스 로직
            await Task.CompletedTask;
        }

        // 큐에 비동기적으로 Command를 넣음
        protected override async Task ProcessAsync(TCommand command)
        {
            _logger.LogInformation("Queuing command...");
            await _queueService.EnqueueAsync(command);
        }
    }

    public interface IQueueService
    {
        Task EnqueueAsync<TCommand>(TCommand command);
    }

}