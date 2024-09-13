using MediationAPIGateWay.Service.도서;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using 도서Infra;
using 仁도서관련자.Query;

namespace MediationAPIGateWay.Handlr.사서.Query
{
    public class Search도서QueryHandler : IRequestHandler<Search도서Query, List<도서Response>>
    {
        private readonly 도서DbContext _dbContext;
        private readonly 도서점수Service _도서점수Service;

        public Search도서QueryHandler(도서DbContext dbContext, 도서점수Service 도서점수Service)
        {
            _dbContext = dbContext;
            _도서점수Service = 도서점수Service;
        }

        public async Task<List<도서Response>> Handle(Search도서Query query, CancellationToken cancellationToken)
        {
            // 도서 목록을 데이터베이스에서 가져옴
            var 도서목록 = await _dbContext.도서들
                .Include(b => b.도서적재)
                .Where(b => query.키워드목록.Any(키워드 =>
                    b.제목.Contains(키워드) ||
                    b.내용.Contains(키워드)))
                .ToListAsync(cancellationToken);

            // 도서 목록을 키워드 목록을 기반으로 정렬
            var 도서Responses = 도서목록.Select(book => new 도서Response
            {
                Id = book.Id,
                제목 = book.제목,
                저자 = book.저자,
                내용 = book.내용,
                적재일 = book.도서적재.적재일
            }).ToList();

            // 도서점수Service를 사용해 도서 점수 계산 후 정렬
            var 정렬된목록 = _도서점수Service.정렬된도서목록(도서Responses, query.키워드목록);

            return 정렬된목록;
        }
    }
}
