using MediatR;
using Microsoft.EntityFrameworkCore;
using 생산Infra;
using 仁농협.Command;

namespace 仁농협.CommandHandlr
{
    public class Approve근로신청들CommandHandler : IRequestHandler<Approve근로신청들Command, Unit>
    {
        private readonly 생산DbContext _context;

        public Approve근로신청들CommandHandler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Approve근로신청들Command request, CancellationToken cancellationToken)
        {
            // Step 1: 근로신청 리스트 조회
            var 근로신청들 = await _context.근로신청들
                .Where(x => request.근로신청Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (근로신청들 == null || !근로신청들.Any())
            {
                throw new ApplicationException("근로신청을 찾을 수 없습니다.");
            }

            // Step 2: 각 근로신청에 대해 결제 상태 확인 및 승인 처리
            foreach (var 신청 in 근로신청들)
            {
                if (신청.보증금납부여부)
                {
                    신청.승인상태 = true;  // 승인 상태로 변경
                    신청.승인자Id = request.농협담당자Id;
                    신청.승인일자 = DateTime.UtcNow;
                }
                else
                {
                    // 보증금이 납부되지 않은 경우, 해당 신청은 승인하지 않음
                    continue;
                }
            }

            // Step 3: 데이터베이스에 저장
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
