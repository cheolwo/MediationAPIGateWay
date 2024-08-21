using MediatR;
using 매칭Infra;
using 仁매칭자.Command;

namespace MediationAPIGateWay.Handlr.매칭자
{
    public class Create스킨십선호CommandHandler : IRequestHandler<Create스킨십선호Command, Unit>
    {
        private readonly 매칭DbContext _context;

        public Create스킨십선호CommandHandler(매칭DbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Create스킨십선호Command request, CancellationToken cancellationToken)
        {
            // 스킨십 선호 정보를 생성 및 저장
            var 선호정보 = new 스킨십선호정보
            {
                사용자Id = request.사용자Id,
                최소허용스킨십 = request.최소허용스킨십,
                최대허용스킨십 = request.최대허용스킨십,
                배차Id = request.배차Id, // 배차 ID 저장
                좌석번호 = request.좌석번호 // 좌석 번호 저장
            };

            // DB에 추가
            _context.스킨십선호정보들.Add(선호정보);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
