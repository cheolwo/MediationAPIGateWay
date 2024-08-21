using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using 국토교통부_공공데이터Common;

namespace MediationAPIGateWay.Service
{
    public class 데이터초기화Service : IHostedService
    {
        private readonly 공동주택DbContext _공동주택Context;
        private readonly ElasticsearchClient _elasticClient;

        public 데이터초기화Service(공동주택DbContext 공동주택Context, ElasticsearchClient elasticClient)
        {
            _공동주택Context = 공동주택Context;
            _elasticClient = elasticClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 공동주택 데이터를 로드하여 Elasticsearch에 저장
            var 공동주택목록 = await _공동주택Context.공동주택목록.ToListAsync();

            foreach (var 공동주택 in 공동주택목록)
            {
                await _elasticClient.IndexAsync(공동주택, idx => idx.Index("공동주택"));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
