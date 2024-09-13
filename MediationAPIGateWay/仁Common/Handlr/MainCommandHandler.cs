using MediatR;
using 仁Common.Command.Queue;
using 仁Common.Command;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using 사용자Infra;
using 仁Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Polly;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace 仁Common.Handlr
{
    public abstract class MainCommandHandler<TCommand> : UserCommandHandler<TCommand>
    where TCommand : IRequest<Unit>, IQueueableCommand, IUserCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStorageDecisionService _storageDecisionService;
        private readonly ILogger<MainCommandHandler<TCommand>> _logger;
        private readonly ICommandQueuePublisher _queuePublisher;
        protected readonly IMongoCollection<사용자> _userCollection;
        private readonly INotificationService _notificationService;
        private readonly IDatabase _redisDb;
        private readonly IAsyncPolicy _retryPolicy;
        private readonly TimeSpan _transactionTimeout;
        private readonly TimeSpan _redisTtl; // Redis TTL

        protected MainCommandHandler(LockService lockService,
                                     IMongoDatabase mongoDatabase,
                                     IServiceProvider serviceProvider,
                                     IStorageDecisionService storageDecisionService,
                                     ILogger<MainCommandHandler<TCommand>> logger,
                                     INotificationService notificationService,
                                     ICommandQueuePublisher queuePublisher,
                                     IConnectionMultiplexer redis,
                                     IConfiguration configuration,
                                     IAsyncPolicy retryPolicy)
            : base(lockService, logger)
        {
            _userCollection = mongoDatabase.GetCollection<사용자>("Users");
            _serviceProvider = serviceProvider;
            _storageDecisionService = storageDecisionService;
            _logger = logger;
            _queuePublisher = queuePublisher;
            _redisDb = redis.GetDatabase();
            _retryPolicy = retryPolicy; // 재시도 정책 주입
            _notificationService = notificationService;
            // 구성 파일에서 트랜잭션 타임아웃 및 Redis TTL 설정 읽기
            _transactionTimeout = TimeSpan.FromSeconds(configuration.GetValue<int>("TransactionTimeout"));
            _redisTtl = TimeSpan.FromMinutes(configuration.GetValue<int>("Redis:TTL"));

        }

        protected override async Task ProcessCommandAsync(TCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Main processing {typeof(TCommand).Name}");

            using (var scope = new TransactionScope(TransactionScopeOption.Required, _transactionTimeout, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 1. 동적으로 결정된 DbContext에서 작업 수행
                    var dbContextType = _storageDecisionService.DecideDbContext(request);
                    var dbContext = (DbContext)_serviceProvider.GetRequiredService(dbContextType);

                    await HandleWithDbContext(dbContext, request, cancellationToken);

                    // 2. MongoDB에서 사용자 정보 갱신
                    await _retryPolicy.ExecuteAsync(() => UpdateUserInMongoDbAsync(request, cancellationToken));

                    // 3. Redis에 데이터 캐싱
                    await _retryPolicy.ExecuteAsync(() => CacheUserDataInRedisAsync(request));

                    // 4. 다음 단계에 보낼 Command 생성 및 큐에 발행
                    var nextCommand = CreateNextCommand(request);
                    _queuePublisher.PublishToQueue(nextCommand);

                    // 5. 트랜잭션 커밋
                    scope.Complete();

                    _logger.LogInformation($"Successfully processed {typeof(TCommand).Name} with {dbContextType.Name}, updated MongoDB, and cached in Redis.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing {typeof(TCommand).Name}, transaction will be rolled back.");

                    // 사용자 및 관리자에게 알림 전송
                    await _notificationService.NotifyUserAsync(request.UserId, "Your order processing failed. Please try again.");
                    await _notificationService.NotifyAdminAsync($"Transaction failed for {typeof(TCommand).Name}. Details: {ex.Message}");

                    throw;
                }
            }
        }

        private async Task UpdateUserInMongoDbAsync(TCommand request, CancellationToken cancellationToken)
        {
            var filter = Builders<사용자>.Filter.Eq(u => u.Id, request.사용자.Id);

            var updateDefinitions = new List<UpdateDefinition<사용자>>
        {
            Builders<사용자>.Update.Set(u => u.이름, request.사용자.이름),
            Builders<사용자>.Update.Set(u => u.성별, request.사용자.성별),
            Builders<사용자>.Update.Set(u => u.전화번호, request.사용자.전화번호)
        };

            var jsonFields = new Dictionary<Expression<Func<사용자, object>>, string>
        {
            { u => u.주문자Json, request.사용자.주문자Json },
            { u => u.생산자Json, request.사용자.생산자Json },
            { u => u.근로자Json, request.사용자.근로자Json },
            { u => u.배송자Json, request.사용자.배송자Json },
            { u => u.후기목록Json, request.사용자.후기목록Json },
            { u => u.근로신청목록Json, request.사용자.근로신청목록Json },
            { u => u.매칭신청목록Json, request.사용자.매칭신청목록Json },
            { u => u.사용자결제목록Json, request.사용자.사용자결제목록Json }
        };

            foreach (var field in jsonFields)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    updateDefinitions.Add(Builders<사용자>.Update.Set(field.Key, field.Value));
                }
            }

            var update = Builders<사용자>.Update.Combine(updateDefinitions);
            await _userCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }

        private async Task CacheUserDataInRedisAsync(TCommand request)
        {
            var cacheKey = $"User:{request.사용자.Id}";
            var serializedData = JsonConvert.SerializeObject(request.사용자);

            await _redisDb.StringSetAsync(cacheKey, serializedData, _redisTtl);
            _logger.LogInformation($"Successfully cached user data in Redis with key {cacheKey}.");
        }

        protected abstract Task HandleWithDbContext(DbContext dbContext, TCommand request, CancellationToken cancellationToken);
        protected abstract IQueueableCommand CreateNextCommand(TCommand request);
    }

}
