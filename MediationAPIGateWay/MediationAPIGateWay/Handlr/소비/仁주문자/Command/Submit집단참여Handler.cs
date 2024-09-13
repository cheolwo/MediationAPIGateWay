using MediatR;
using 주문Common.Model;
using 仁주문자.For주문자.Command;
using 仁Common; // I결제Service가 있는 네임스페이스
using MongoDB.Driver;
using MediationAPIGateWay.Repository;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command
{
    public class Submit집단참여Handler : IRequestHandler<Submit집단참여Command, Unit>
    {
        private readonly 주문자집단Repository _집단Repository;
        private readonly I결제Service _결제Service;

        public Submit집단참여Handler(
            주문자집단Repository 집단Repository,
            I결제Service 결제Service)
        {
            _집단Repository = 집단Repository;
            _결제Service = 결제Service;
        }

        public async Task<Unit> Handle(Submit집단참여Command command, CancellationToken cancellationToken)
        {
            // 1. 보증금 결제 검증
            if (command.보증금 > 0)
            {
                var 결제성공 = await _결제Service.결제검증(command.결제요청Dto);
                if (!결제성공)
                {
                    throw new Exception("결제 검증 실패");
                }
            }

            // 2. RDBMS에서 집단 정보 확인
            var 집단 = await _집단Repository.Get집단Async(command.집단코드);
            if (집단 == null)
            {
                throw new Exception("집단이 존재하지 않습니다.");
            }

            // 3. MongoDB에서 사용자 정보 확인
            var 사용자 = await _집단Repository.GetUserAsync(command.사용자Id);
            if (사용자 == null)
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            // 4. 주문자가 이미 집단에 속해 있는지 확인
            if (집단.주문자들 != null && 집단.주문자들.Any(x => x.주문자Id == command.주문자Id))
            {
                throw new Exception("이미 집단에 참여한 주문자입니다.");
            }

            // 5. RDBMS에서 주문자 정보 추가
            var 주문자 = new 주문자
            {
                주문자Id = command.주문자Id,
                이름 = 사용자.이름,
                연락처 = 사용자.전화번호
            };

            await _집단Repository.Add주문자To집단Async(command.집단코드, 주문자);

            // 6. MongoDB에서 사용자 정보 업데이트 (주문자집단 추가)
            await _집단Repository.UpdateUser집단Async(command.사용자Id, 집단);

            return Unit.Value;
        }
    }
}
