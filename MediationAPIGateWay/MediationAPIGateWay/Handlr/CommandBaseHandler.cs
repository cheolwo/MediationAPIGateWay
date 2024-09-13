using MediatR;

namespace MediationAPIGateWay.Handlr
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(string id);
    }

    public abstract class CommandHandlerBase<TCommand, TEntity, TRepository> : IRequestHandler<TCommand, Unit>
    where TCommand : IRequest<Unit>
    where TEntity : class
    where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly IConfiguration _configuration;

        public CommandHandlerBase(TRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            // HMAC 검증 (공통 검증 로직)
            if (!VerifyHMAC(command))
            {
                throw new ApplicationException("HMAC 검증 실패");
            }

            // 비동기 큐 사용 여부를 결정
            if (ShouldUseAsyncQueue(command))
            {
                await ProcessAsync(command);
            }
            else
            {
                // 동기적으로 Command 처리
                await HandleCommandAsync(command, cancellationToken);
            }

            return Unit.Value;
        }

        // HMAC 검증 로직
        protected virtual bool VerifyHMAC(TCommand command)
        {
            // 기본 HMAC 검증
            return true;
        }

        // 비동기 큐 처리 로직
        protected virtual Task ProcessAsync(TCommand command)
        {
            // 비동기 큐에 넣는 기본 로직 (필요시 구현)
            return Task.CompletedTask;
        }

        // Command를 실제로 처리하는 로직
        protected abstract Task HandleCommandAsync(TCommand command, CancellationToken cancellationToken);

        // 비동기 큐를 사용할지 여부를 결정하는 메서드
        protected bool ShouldUseAsyncQueue(TCommand command)
        {
            // appsettings.json에서 Command에 대한 설정을 가져옴
            var commandName = command.GetType().Name;
            var useAsyncQueue = _configuration.GetSection($"CommandSettings:{commandName}:UseAsyncQueue").Value;

            // 설정된 값이 true면 비동기 처리, 아니면 동기 처리
            return bool.TryParse(useAsyncQueue, out bool result) && result;
        }
    }
}
