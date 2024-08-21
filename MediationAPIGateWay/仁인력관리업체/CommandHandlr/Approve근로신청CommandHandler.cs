using MediatR;
using Microsoft.EntityFrameworkCore;
using 생산Infra;
using 仁농협.Command;

namespace 仁농협.CommandHandlr
{
    public class Approve근로신청CommandHandler : IRequestHandler<Approve근로신청Command, Unit>
    {
        private readonly 생산DbContext _context;

        public Approve근로신청CommandHandler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Approve근로신청Command request, CancellationToken cancellationToken)
        {
            // Step 1: 근로신청 조회
            var 근로신청 = await _context.근로신청들
                .FirstOrDefaultAsync(x => x.Id == request.근로신청Id, cancellationToken);

            if (근로신청 == null)
            {
                // 근로신청이 없으면 예외 처리
                throw new ApplicationException("해당 근로신청을 찾을 수 없습니다.");
            }

            // Step 2: 결제 상태 확인
            if (!근로신청.보증금납부여부)
            {
                // 보증금이 납부되지 않았다면 승인 불가
                throw new ApplicationException("보증금이 납부되지 않은 근로신청입니다.");
            }

            // Step 3: 승인 처리
            근로신청.승인상태 = true;  // 승인 상태로 변경
            근로신청.승인자Id = request.농협담당자Id;
            근로신청.승인일자 = DateTime.UtcNow;

            // Step 4: 데이터베이스에 저장
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
