using MediatR;
using 仁Common.Command.Queue;
using 仁Common.Command;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using 仁Common.Services;

namespace 仁Common.Handlr
{
    public abstract class BeforeCommandHandler<TCommand> : UserCommandHandler<TCommand>
    where TCommand : IRequest<Unit>, IQueueableCommand
    {
        private readonly ICommandQueuePublisher _queuePublisher;

        protected BeforeCommandHandler(LockService lockService, ILogger<BeforeCommandHandler<TCommand>> logger, ICommandQueuePublisher queuePublisher)
            : base(lockService, logger)
        {
            _queuePublisher = queuePublisher;
        }

        protected override async Task ProcessCommandAsync(TCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Before processing {typeof(TCommand).Name}");

            // 정합성 검사 로직
            ValidateRequest(request);

            // 다음 단계에 보낼 Command 생성 및 큐에 발행
            var nextCommand = CreateNextCommand(request);
            _queuePublisher.PublishToQueue(nextCommand);

            _logger.LogInformation($"Successfully enqueued {typeof(TCommand).Name} for next processing stage.");
        }

        // 하위 클래스에서 구현해야 할 정합성 검사 로직
        protected abstract void ValidateRequest(TCommand request);

        // 하위 클래스에서 구현해야 할 다음 명령 생성 로직
        protected abstract IQueueableCommand CreateNextCommand(TCommand request);
    }

    public abstract class AfterCommandHandler<TCommand> : UserCommandHandler<TCommand>
    where TCommand : IRequest<Unit>, IQueueableCommand
    {
        private readonly IDatabase _redisDb;

        protected AfterCommandHandler(LockService lockService,
                                      ILogger<AfterCommandHandler<TCommand>> logger,
                                      IConnectionMultiplexer redis)
            : base(lockService, logger)
        {
            _redisDb = redis.GetDatabase();
        }

        protected override async Task ProcessCommandAsync(TCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"After processing {typeof(TCommand).Name}");

            // Redis 캐싱 로직
            var cacheKey = $"User:{request.UserId}";
            var serializedData = JsonConvert.SerializeObject(request);
            await _redisDb.StringSetAsync(cacheKey, serializedData);

            _logger.LogInformation($"Successfully cached {typeof(TCommand).Name} in Redis.");
        }
    }
}
