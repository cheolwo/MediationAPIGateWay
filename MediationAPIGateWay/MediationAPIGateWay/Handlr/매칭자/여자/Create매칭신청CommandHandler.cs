using MediationAPIGateWay.Service.매칭;
using MediatR;
using 매칭Infra;
using 仁매칭자.Command;

namespace MediationAPIGateWay.Handlr.매칭자.여자
{
    public class Create매칭신청CommandHandler : IRequestHandler<Create매칭신청Command, Unit>
    {
        private readonly 이성DbContext _context;
        private readonly IMatchingService _matchingService;  // 변경된 인터페이스 사용

        public Create매칭신청CommandHandler(이성DbContext context, IMatchingService matchingService)
        {
            _context = context;
            _matchingService = matchingService;
        }

        public async Task<Unit> Handle(Create매칭신청Command request, CancellationToken cancellationToken)
        {
            // Step 1: 선호 정보 생성 및 저장
            var 선호정보 = new 매칭선호정보
            {
                사용자Id = request.사용자Id,
                최소나이 = request.최소나이,
                최대나이 = request.최대나이,
                연상연하선호 = request.연상연하선호,
                기타선호사항 = request.기타선호사항
            };

            // Step 2: 매칭 정보 생성
            var 매칭신청 = new 매칭
            {
                사용자Id = request.사용자Id,
                신청일자 = DateTime.UtcNow,
                매칭완료여부 = false,
                선호정보 = 선호정보
            };

            // Step 3: 매칭 서비스 호출
            var 매칭결과 = await _matchingService.MatchAsync(매칭신청, cancellationToken);

            if (매칭결과 == null)
            {
                throw new ApplicationException("적합한 매칭 상대를 찾을 수 없습니다.");
            }

            // 매칭이 완료된 경우 상태 업데이트
            매칭신청.매칭완료여부 = true;

            // Step 4: 매칭 데이터 저장
            _context.매칭선호정보들.Add(선호정보);
            _context.매칭들.Add(매칭신청);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
