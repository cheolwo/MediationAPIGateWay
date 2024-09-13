using Nest;
using 주문Common.Model;

namespace MediationAPIGateWay.Service.검색
{
    public interface I주문자집단QueryService
    {
        Task IndexOrderGroupAsync(주문자집단 orderGroup);
        Task<List<주문자집단>> GetMatchingOrderGroupsSplitAddressAsync(string address);
        Task<List<주문자집단>> GetMatchingOrderGroupsMultiMatchAsync(string address);
    }

    public class 주문자SearchService : I주문자집단QueryService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<주문자SearchService> _logger;

        public 주문자SearchService(IElasticClient elasticClient, ILogger<주문자SearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        // 엘라스틱 서치에 주문자집단 데이터를 인덱싱
        public async Task IndexOrderGroupAsync(주문자집단 orderGroup)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(orderGroup);

            if (!indexResponse.IsValid)
            {
                _logger.LogError($"엘라스틱 서치 인덱싱 실패: {indexResponse.OriginalException.Message}");
                throw new Exception("엘라스틱 서치 인덱싱에 실패했습니다.");
            }

            _logger.LogInformation($"주문자집단 {orderGroup.집단코드}가 성공적으로 인덱싱되었습니다.");
        }

        // 키워드를 나눠서 검색 (Split Address)
        public async Task<List<주문자집단>> GetMatchingOrderGroupsSplitAddressAsync(string address)
        {
            var keywords = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var searchResponse = await _elasticClient.SearchAsync<주문자집단>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            keywords.Select(keyword => (Func<QueryContainerDescriptor<주문자집단>, QueryContainer>)(m => m
                                .Match(mt => mt
                                    .Field(f => f.법정동주소)
                                    .Query(keyword)
                                )
                            )).ToArray()
                        )
                    )
                )
                .Size(10) // 원하는 결과 수 제한
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception("엘라스틱 서치 조회 실패.");
            }

            return searchResponse.Documents.ToList();
        }

        // MultiMatch 쿼리를 통해 여러 필드에서 일치하는 주문자집단 검색
        public async Task<List<주문자집단>> GetMatchingOrderGroupsMultiMatchAsync(string address)
        {
            var keywords = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var searchResponse = await _elasticClient.SearchAsync<주문자집단>(s => s
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(f => f
                            .Field(ff => ff.법정동주소)
                            .Field(ff => ff.단지명)  // 다른 필드도 함께 검색 가능
                        )
                        .Query(string.Join(' ', keywords))
                    )
                )
                .Size(10) // 원하는 결과 수 제한
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception("엘라스틱 서치 조회 실패.");
            }

            return searchResponse.Documents.ToList();
        }
    }
}
