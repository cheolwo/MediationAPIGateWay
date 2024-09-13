using MediationAPIGateWay.Service.검색;
using Microsoft.EntityFrameworkCore;
using 근로Infra;

namespace MediationAPIGateWay.Service.Hosted
{
    public class 근로매칭Indexer : IHostedService
    {
        private readonly 근로DbContext _dbContext;
        private readonly I가용인력QueryService _elasticSearchService;
        private readonly ILogger<근로매칭Indexer> _logger;

        public 근로매칭Indexer(근로DbContext dbContext, I가용인력QueryService elasticSearchService, 
            ILogger<근로매칭Indexer> logger)
        {
            _dbContext = dbContext;
            _elasticSearchService = elasticSearchService;
            _logger = logger;
        }

        // 서비스 시작 시 대기 중인 근로신청 데이터를 엘라스틱 서치에 인덱싱
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WorkerApplicationIndexer: 시작 중...");

            try
            {
                // 대기 중인 근로신청 데이터만 가져오기 (신청상태가 '대기 중'인 것)
                var pendingApplications = await _dbContext.근로신청정보
                    .Where(a => a.신청상태 == "대기 중")
                    .ToListAsync();

                // 각 근로신청 데이터를 엘라스틱 서치에 인덱싱
                foreach (var application in pendingApplications)
                {
                    await _elasticSearchService.IndexWorkerApplicationAsync(application);
                }

                _logger.LogInformation("모든 대기 중인 근로신청 데이터가 인덱싱되었습니다.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"WorkerApplicationIndexer 오류: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WorkerApplicationIndexer: 종료 중...");
            return Task.CompletedTask;
        }
    }
}
