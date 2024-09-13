using MediatR;
using Microsoft.EntityFrameworkCore;
using 근로Infra;
using 仁생산자.Query;

namespace MediationAPIGateWay.Handlr.생산.생산자
{
    public class Get가용인력QueryHandler : IRequestHandler<Get가용인력Query, List<근로신청>>
    {
        private readonly 근로DbContext _context;

        public Get가용인력QueryHandler(근로DbContext context)
        {
            _context = context;
        }

        public async Task<List<근로신청>> Handle(Get가용인력Query request, CancellationToken cancellationToken)
        {
            // 근로자의 위치가 생산자의 지역 또는 작업 위치와 인접한 근로자를 필터링
            var 근로자목록 = await _context.근로신청정보
                .Where(g => g.지역 == request.지역)
                .ToListAsync(cancellationToken);

            return 근로자목록;
        }
    }

}
