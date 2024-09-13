using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using 仁Common.Command.Queue;
using 仁Common.Command;
using 仁Common.Handlr;
using 仁Common.Services;
using 仁주문자.For주문자.Command;
using 주문Infra.Model;
using 仁Common;
using Polly;
using StackExchange.Redis;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command.개별주문
{
    public class MainCreate개별주문CommandHandler : MainCommandHandler<Create개별주문Command>
    {
        private readonly I결제Service _결제Service;

        public MainCreate개별주문CommandHandler(LockService lockService,
                                                  IMongoDatabase mongoDatabase,
                                                  IServiceProvider serviceProvider,
                                                  IStorageDecisionService storageDecisionService,
                                                  ILogger<MainCreate개별주문CommandHandler> logger,
                                                  INotificationService notificationService,
                                                  ICommandQueuePublisher queuePublisher,
                                                  IConnectionMultiplexer redis,
                                                  IConfiguration configuration,
                                                  IAsyncPolicy retryPolicy,
                                                  I결제Service 결제Service)
            : base(lockService, mongoDatabase, serviceProvider, storageDecisionService, logger, notificationService, queuePublisher, redis, configuration, retryPolicy)
        {
            _결제Service = 결제Service;
        }

        protected override async Task HandleWithDbContext(DbContext dbContext, Create개별주문Command request, CancellationToken cancellationToken)
        {
            // 주문 데이터를 처리하는 로직
            var order = new 개별주문상품
            {
                상품코드 = request.상품Id.ToString(),
                상품명 = request.상품명,
                설명 = $"Order for {request.수량} units of {request.상품명}",
                가격 = request.가격,
                주문상태 = 주문상태.미확인,
                주문자Id = request.사용자Id
            };

            dbContext.Set<개별주문상품>().Add(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Order {order.상품코드} processed and saved to {dbContext.GetType().Name}.");
        }

        protected override IQueueableCommand CreateNextCommand(Create개별주문Command request)
        {
            return new AfterCreate개별주문Command
            {
                사용자Id = request.사용자Id,
                상품Id = request.상품Id,
                상품명 = request.상품명,
                가격 = request.가격,
                수량 = request.수량,
                배송주소 = request.배송주소,
                결제요청Dto = request.결제요청Dto
            };
        }
    }

}
