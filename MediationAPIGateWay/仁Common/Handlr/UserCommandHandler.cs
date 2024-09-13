using MediatR;
using Microsoft.Extensions.Logging;
using 仁Common.Command.Queue;
using 仁Common.Services;

namespace 仁Common.Handlr
{
    public abstract class UserCommandHandler<TCommand> : IRequestHandler<TCommand, Unit>
     where TCommand : IRequest<Unit>, IQueueableCommand
    {
        private readonly LockService _lockService;
        protected readonly ILogger<UserCommandHandler<TCommand>> _logger;

        protected UserCommandHandler(LockService lockService, ILogger<UserCommandHandler<TCommand>> logger)
        {
            _lockService = lockService;
            _logger = logger;
        }

        public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var lockKey = GetUserLockKey(request.UserId);

            if (await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    // 각 핸들러에서 구현해야 할 구체적인 작업
                    await ProcessCommandAsync(request, cancellationToken);
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing {typeof(TCommand).Name} for User: {request.UserId}");
                    throw;
                }
                finally
                {
                    // 락 해제
                    await _lockService.ReleaseLockAsync(lockKey);
                }
            }
            else
            {
                _logger.LogWarning($"Could not acquire lock for {typeof(TCommand).Name} for User: {request.UserId}");
                throw new InvalidOperationException("Resource is locked");
            }
        }

        protected abstract Task ProcessCommandAsync(TCommand request, CancellationToken cancellationToken);

        private string GetUserLockKey(string userId)
        {
            return $"User:{userId}:Lock";
        }
    }

}
