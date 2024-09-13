using 생산Infra.Model;
using 仁생산자.Command;
using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using 仁Common.Handlr;
using 仁Common.Services;
using 仁매칭infra.Repository;
using 仁매칭infra;
using MongoDB.Bson;

namespace MediationAPIGateWay.Handlr.생산.생산자.Command
{
    public class Create인력매칭신청CommandHandler : GeneralCommandHandler<Create인력매칭신청Command>
    {
        protected readonly I근로인력매칭Repository _근로인력매칭Repository;
        public Create인력매칭신청CommandHandler(
            IServiceProvider serviceProvider,
            IStorageDecisionService storageDecisionService,
            ILogger<Create인력매칭신청CommandHandler> logger,
            I근로인력매칭Repository 근로인력매칭Repository,
            ElasticsearchClient elasticsearchClient)
            : base(serviceProvider, storageDecisionService, logger, elasticsearchClient)
        {
            _근로인력매칭Repository = 근로인력매칭Repository;
        }

        protected override async Task HandleWithDbContext(DbContext dbContext, Create인력매칭신청Command command, CancellationToken cancellationToken)
        {
            var 인력매칭신청 = new 생산자인력매칭신청
            {
                사용자Id = command.사용자Id,
                근무지Id = command.근무지Id,
                생산자Id = command.생산자Id,
                근무시작일 = command.근무시작일,
                근무종료일 = command.근무종료일
            };

            dbContext.Add(인력매칭신청);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        protected override async Task SaveDataToMongoDb(Create인력매칭신청Command command, CancellationToken cancellationToken)
        {
            await _근로인력매칭Repository.InsertAsync(new 근로인력매칭
            {
                생산자Id = command.생산자Id,
                근무지Id = command.근무지Id,
                매칭신청일자 = DateTime.UtcNow
            });;
        }

        protected override async Task IndexDataInElasticSearch(Create인력매칭신청Command command, CancellationToken cancellationToken)
        {
            // MongoDB에서 최근에 추가된 인력매칭 정보 조회
            var 인력매칭Data = await _근로인력매칭Repository.GetLastInsertedAsync(command.생산자Id, cancellationToken);

            if (인력매칭Data == null)
            {
                _logger.LogError("No data found in MongoDB for indexing: 생산자Id {생산자Id}", command.생산자Id);
                return;
            }

            // Elasticsearch에 데이터 인덱싱
            var indexResponse = await _ElasticsearchClient.IndexAsync(인력매칭Data, idx => idx
                .Index("근로인력매칭-index") // 인덱스 이름 지정
                .Refresh(Elastic.Clients.Elasticsearch.Refresh.True)); // 인덱싱 후 즉시 검색 가능하도록 설정

            if (!indexResponse.IsValid)
            {
                _logger.LogError("Failed to index data in Elasticsearch: {Error}", indexResponse.ServerError?.Error?.Reason);
                throw new InvalidOperationException("Elasticsearch indexing failed.");
            }

            _logger.LogInformation("Successfully indexed 근로인력매칭 data in Elasticsearch for 생산자Id {생산자Id}", command.생산자Id);
        }
    }
}