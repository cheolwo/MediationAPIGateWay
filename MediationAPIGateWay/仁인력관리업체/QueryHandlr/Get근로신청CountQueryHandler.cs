using MediatR;
using Microsoft.EntityFrameworkCore;
using 생산Infra;
using 仁농협.Query;

namespace 仁농협.QueryHandlr
{
    public class Get근로신청CountQueryHandler : IRequestHandler<Get근로신청CountQuery, int>
    {
        private readonly 생산DbContext _context;

        public Get근로신청CountQueryHandler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(Get근로신청CountQuery request, CancellationToken cancellationToken)
        {
            // 해당 근무지에 대한 근로신청 수를 계산
            // SQL: SELECT COUNT(*) FROM 근로신청 WHERE 근무지Id = @근무지Id;
            var 신청자수 = await _context.근로신청들
                .Where(x => x.근무지Id == request.근무지Id)
                .CountAsync(cancellationToken);

            return 신청자수;
        }
    }
}
