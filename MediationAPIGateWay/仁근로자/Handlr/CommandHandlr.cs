using MediatR;
using 매칭Infra;
using 생산Infra;
using 생산Infra.Model;
using 셔틀Infra;
using 셔틀Infra.Model;
using 仁Common;
using 仁Common.근로자;

namespace 仁근로자.Handlr
{
    public class Create근로신청CommandHandler : IRequestHandler<Create근로신청Command, Unit>
    {
        private readonly 생산DbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly 셔틀DbContext _셔틀Context;

        public Create근로신청CommandHandler(생산DbContext context, IPaymentService paymentService, 셔틀DbContext 셔틀Context)
        {
            _context = context;
            _paymentService = paymentService;
            _셔틀Context = 셔틀Context;
        }

        public async Task<Unit> Handle(Create근로신청Command request, CancellationToken cancellationToken)
        {
            // Step 1: 결제 확인 및 검증
            var 결제성공여부 = await _paymentService.VerifyPayment(request.결제요청Dto);

            if (!결제성공여부)
            {
                throw new ApplicationException("결제가 완료되지 않았습니다.");
            }

            // Step 2: 보증금액 검증
            var 설정된보증금액 = 100000M; // 예시로 설정된 보증금액
            if (request.결제요청Dto.결제금액 != 설정된보증금액)
            {
                throw new ApplicationException("결제된 금액이 설정된 보증금액과 일치하지 않습니다.");
            }

            // Step 3: 근로신청 데이터 생성
            var 신청 = new 근로신청
            {
                사용자Id = request.사용자Id,
                근무지Id = request.근무지Id,
                근무시작일 = request.근무시작일,
                근무종료일 = request.근무종료일,
                보증금납부여부 = true,
                설정보증금액 = 설정된보증금액,
            };

            _context.근로신청들.Add(신청);
            await _context.SaveChangesAsync(cancellationToken);

            // Step 4: 셔틀버스 정보 처리
            if (!request.자차여부 && request.셔틀버스배차요청Dto != null)
            {
                var 셔틀정보 = new 셔틀버스배차
                {
                    사용자Id = request.사용자Id,
                    근무지Id = request.근무지Id,
                    지하철역번호 = request.셔틀버스배차요청Dto.지하철역번호,
                    셔틀버스번호 = request.셔틀버스배차요청Dto.셔틀버스번호,
                    배차일자 = DateTime.UtcNow
                };

                _셔틀Context.셔틀버스배차들.Add(셔틀정보);
                await _셔틀Context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
