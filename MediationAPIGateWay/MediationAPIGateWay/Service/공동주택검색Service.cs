using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using 국토교통부_공공데이터Common.Model;
using 仁주문자.주문자.Query;

namespace MediationAPIGateWay.Service
{
    public class 공동주택검색Service
    {
        private readonly ElasticsearchClient _elasticClient;

        public 공동주택검색Service(ElasticsearchClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<공동주택>> 검색Async(공동주택조회Query query)
        {
            var mustQueries = new List<Elastic.Clients.Elasticsearch.QueryDsl.Query>();

            if (!string.IsNullOrEmpty(query.단지코드))
            {
                mustQueries.Add(new TermQuery(new Field("단지코드.keyword"))
                {
                    Value = query.단지코드
                });
            }

            if (!string.IsNullOrEmpty(query.단지명))
            {
                mustQueries.Add(new MatchQuery(new Field("단지명"))
                {
                    Query = query.단지명
                });
            }

            if (!string.IsNullOrEmpty(query.시도))
            {
                mustQueries.Add(new TermQuery(new Field("시도.keyword"))
                {
                    Value = query.시도
                });
            }

            if (!string.IsNullOrEmpty(query.시군구))
            {
                mustQueries.Add(new TermQuery(new Field("시군구.keyword"))
                {
                    Value = query.시군구
                });
            }

            if (!string.IsNullOrEmpty(query.읍면동))
            {
                mustQueries.Add(new TermQuery(new Field("읍면동.keyword"))
                {
                    Value = query.읍면동
                });
            }

            if (!string.IsNullOrEmpty(query.리))
            {
                mustQueries.Add(new TermQuery(new Field("리.keyword"))
                {
                    Value = query.리
                });
            }

            if (!string.IsNullOrEmpty(query.법정동코드))
            {
                mustQueries.Add(new TermQuery(new Field("법정동코드.keyword"))
                {
                    Value = query.법정동코드
                });
            }

            var searchResponse = await _elasticClient.SearchAsync<공동주택>(s => s
                .Index("공동주택")
                .Query(q => q.Bool(b => b.Must(mustQueries.ToArray())))
            );

            return searchResponse.Documents.ToList();
        }
    }
}
