using Nest;
using 근로Infra;

namespace MediationAPIGateWay.Service.검색
{
    public interface I가용인력QueryService
    {
        Task IndexWorkerApplicationAsync(근로신청 workerApplication);
        Task<List<근로신청>> GetMatchingWorkersSplitAddressAsync(string address);
        Task<List<근로신청>> GetMatchingWorkersMultiMatchAsync(string address);
    }
    public class 생산자SearchService : I가용인력QueryService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<생산자SearchService> _logger;

        public 생산자SearchService(IElasticClient elasticClient, ILogger<생산자SearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        // 엘라스틱 서치에 근로신청 데이터를 인덱싱
        public async Task IndexWorkerApplicationAsync(근로신청 workerApplication)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(workerApplication);

            if (!indexResponse.IsValid)
            {
                _logger.LogError($"엘라스틱 서치 인덱싱 실패: {indexResponse.OriginalException.Message}");
                throw new Exception("엘라스틱 서치 인덱싱에 실패했습니다.");
            }

            _logger.LogInformation($"근로신청 {workerApplication.근로신청Id}가 성공적으로 인덱싱되었습니다.");
        }
        public async Task<List<근로신청>> GetMatchingWorkersSplitAddressAsync(string address)
        {
            // 공백으로 분절하여 키워드 배열을 생성
            var keywords = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var searchResponse = await _elasticClient.SearchAsync<근로신청>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            keywords.Select(keyword => (Func<QueryContainerDescriptor<근로신청>, QueryContainer>)(m => m
                                .Match(mt => mt
                                    .Field(f => f.주소)
                                    .Query(keyword)
                                )
                            )).ToArray()
                        )
                    )
                )
                // 일치하는 키워드가 많을수록 상위에 오도록 점수 부여
                .Size(10) // 원하는 결과 수 제한
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception("Failed to search documents in Elasticsearch.");
            }

            return searchResponse.Documents.ToList();
        }
        public async Task<List<근로신청>> GetMatchingWorkersMultiMatchAsync(string address)
        {
            var keywords = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var searchResponse = await _elasticClient.SearchAsync<근로신청>(s => s
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(f => f
                            .Field(ff => ff.주소)
                        // .Field(ff => ff.작업위치)  // 다른 필드도 함께 검색할 수 있습니다.
                        )
                        .Query(string.Join(' ', keywords))  // 모든 키워드를 사용하여 검색
                    )
                )
                .Size(10) // 원하는 결과 수 제한
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception("Failed to search documents in Elasticsearch.");
            }

            return searchResponse.Documents.ToList();
        }
    }
}
