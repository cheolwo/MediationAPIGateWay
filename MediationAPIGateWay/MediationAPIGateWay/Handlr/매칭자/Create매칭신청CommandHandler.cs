using MediatR;
using 매칭Infra;
using 仁매칭자.Command;

namespace MediationAPIGateWay.Handlr.매칭자
{
    public class Create매칭신청CommandHandler : IRequestHandler<Create매칭신청Command, Unit>
    {
        private readonly 이성DbContext _context;

        public Create매칭신청CommandHandler(이성DbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Create매칭신청Command request, CancellationToken cancellationToken)
        {
            // Step 1: 매칭 신청 데이터 생성
            var 매칭신청 = new 매칭
            {
                사용자Id = request.사용자Id,                // 사용자 ID 매핑
                근무지Id = 0,                               // 근무지 ID는 추후 설정 (0으로 초기화)
                신청일자 = DateTime.UtcNow,                 // 신청 일자는 현재 시간으로 설정
                매칭완료여부 = false,                       // 신청 시 매칭완료 여부는 false로 설정
                가능매칭구간 = "임시 구간",                 // 매칭 가능한 구간 (기본 값)
                성별 = request.성별,                        // 선호 성별
                최소나이 = request.최소나이,                // 선호 최소 나이
                최대나이 = request.최대나이,                // 선호 최대 나이
                기타선호사항 = request.기타선호사항,        // 기타 선호 사항
                형식상최소허용스킨십단계 = request.형식상최소허용스킨십단계, // 최소 허용 스킨십 단계
                형식상최대허용스킨십단계 = request.형식상최대허용스킨십단계  // 최대 허용 스킨십 단계
            };

            // Step 2: 매칭 신청 데이터 저장
            _context.매칭들.Add(매칭신청);
            await _context.SaveChangesAsync(cancellationToken);

            // Step 3: 추가 비즈니스 로직 (알림 전송, 매칭 상태 업데이트 등)
            // 필요 시 이곳에 추가 비즈니스 로직 작성 가능.

            return Unit.Value;  // 성공적으로 처리되었음을 반환
        }
    }
}
