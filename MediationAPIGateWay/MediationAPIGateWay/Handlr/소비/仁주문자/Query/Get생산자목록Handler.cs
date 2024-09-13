using MediatR;
using 생산Infra;
using Microsoft.EntityFrameworkCore;
using 仁주문자.For주문자.Query;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Query
{
    public class Get생산자목록Handler : IRequestHandler<Get생산자목록Query, Get생산자목록Response>
    {
        private readonly 생산DbContext _context;

        public Get생산자목록Handler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<Get생산자목록Response> Handle(Get생산자목록Query request, CancellationToken cancellationToken)
        {
            // 농협 ID로 생산자 목록 조회
            var 생산자목록 = await _context.생산자들
                .Where(p => p.농협Id == request.농협Id)
                .Include(p => p.근무지목록) // 생산자의 근무지 목록 포함
                .ToListAsync(cancellationToken);

            return new Get생산자목록Response
            {
                생산자목록 = 생산자목록
            };
        }
    }
}
