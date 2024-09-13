using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using 仁Common.Command;
using 仁Common.Handlr;
using 仁Common.Services;
using 仁주문자.For주문자.Command;
using 仁Common.Command.Queue;
using Polly;
using StackExchange.Redis;
using 주문Infra;
using 주문Infra.Model;
using MediationAPIGateWay.Service;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command.공동주문
{
    public class MainCreate공동주문CommandHandler : MainCommandHandler<Create공동주문Command>
    {
        private readonly 전자서명Service _전자서명Service;

        public MainCreate공동주문CommandHandler(LockService lockService,
                                                 IMongoDatabase mongoDatabase,
                                                 IServiceProvider serviceProvider,
                                                 IStorageDecisionService storageDecisionService,
                                                 ILogger<MainCreate공동주문CommandHandler> logger,
                                                 INotificationService notificationService,
                                                 ICommandQueuePublisher queuePublisher,
                                                 IConnectionMultiplexer redis,
                                                 IConfiguration configuration,
                                                 IAsyncPolicy retryPolicy,
                                                 전자서명Service 전자서명Service)
            : base(lockService, mongoDatabase, serviceProvider, storageDecisionService, logger, notificationService, queuePublisher, redis, configuration, retryPolicy)
        {
            _전자서명Service = 전자서명Service;
        }

        protected override async Task ProcessCommandAsync(Create공동주문Command request, CancellationToken cancellationToken)
        {
            // 전자서명 검증
            var 사용자 = await _userCollection.Find(u => u.Id == request.사용자Id).FirstOrDefaultAsync(cancellationToken);
            if (사용자 == null)
            {
                throw new ArgumentException($"Invalid 사용자 ID: {request.사용자Id}");
            }

            if (!_전자서명Service.VerifySignature(request, 사용자.공개키인증서))
            {
                throw new ApplicationException("전자서명 검증 실패");
            }

            await base.ProcessCommandAsync(request, cancellationToken);
        }

        protected override async Task HandleWithDbContext(DbContext dbContext, Create공동주문Command request, CancellationToken cancellationToken)
        {
            var 주문DbContext = dbContext as 주문DbContext;
            if (주문DbContext == null)
            {
                throw new InvalidOperationException("Invalid DbContext provided.");
            }

            // 공동주문 데이터를 처리하는 로직
            var 공동주문상품들 = request.공동주문상품들.Select(p => new 공동주문상품
            {
                상품코드 = p.상품코드,
                상품명 = p.상품명,
                설명 = p.설명,
                가격 = p.가격,
                발의일자 = DateTime.UtcNow,
                주문상태 = "미확인",
                집단코드 = request.집단코드,
                생산자Id = p.생산자Id
            }).ToList();

            var 주문자집단 = await 주문DbContext.주문자집단들
                .Include(j => j.공동주문상품들)
                .FirstOrDefaultAsync(j => j.집단코드 == request.집단코드, cancellationToken);

            if (주문자집단 == null)
            {
                throw new ArgumentException($"Invalid 집단코드: {request.집단코드}");
            }

            주문자집단.공동주문상품들.AddRange(공동주문상품들);

            await 주문DbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Successfully processed 공동주문 for 집단코드 {request.집단코드} and saved to {dbContext.GetType().Name}.");
        }

        protected override IQueueableCommand CreateNextCommand(Create공동주문Command request)
        {
            // 다음 단계 명령어 생성
            return new AfterCreate공동주문Command(request.집단코드, request.주문자Id, request.공동주문상품들, request.배송주소, request.결제요청Dto, request.사용자Id, request.전자서명);
        }
    }

}
