using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using 仁Common.Command;
using 仁Common.Services;

namespace 仁Common.Handlr
{
    abstract public class GeneralCommandHandler<TCommand> : IRequestHandler<TCommand, Unit>
     where TCommand : IRequest<Unit>, IUserCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStorageDecisionService _storageDecisionService;
        protected readonly ILogger<GeneralCommandHandler<TCommand>> _logger;
        protected readonly ElasticsearchClient _ElasticsearchClient;

        public GeneralCommandHandler(
            IServiceProvider serviceProvider,
            IStorageDecisionService storageDecisionService,
            ILogger<GeneralCommandHandler<TCommand>> logger,
            ElasticsearchClient elasticsearchClient)
        {
            _serviceProvider = serviceProvider;
            _storageDecisionService = storageDecisionService;
            _logger = logger;
            _ElasticsearchClient = elasticsearchClient;
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // DbContext 결정
                var dbContextType = _storageDecisionService.DecideDbContext(command);
                var dbContext = (DbContext)_serviceProvider.GetRequiredService(dbContextType);

                // 데이터 처리
                await HandleWithDbContext(dbContext, command, cancellationToken);

                // MongoDB에 데이터 저장
                await SaveDataToMongoDb(command, cancellationToken);

                // ElasticSearch에 데이터 인덱싱
                await IndexDataInElasticSearch(command, cancellationToken);

                _logger.LogInformation($"Command processed and data indexed for {typeof(TCommand).Name}");
                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process command for {typeof(TCommand).Name}");
                throw;
            }
        }

        protected abstract Task HandleWithDbContext(DbContext dbContext, TCommand command, CancellationToken cancellationToken);
        protected abstract Task SaveDataToMongoDb(TCommand command, CancellationToken cancellationToken);
        protected abstract Task IndexDataInElasticSearch(TCommand command, CancellationToken cancellationToken);
    }
}
