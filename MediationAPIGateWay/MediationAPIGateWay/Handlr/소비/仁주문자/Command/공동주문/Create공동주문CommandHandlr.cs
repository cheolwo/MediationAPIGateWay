using MediatR;
using Newtonsoft.Json;
using 사용자Infra;
using 주문Infra.Model;
using 仁Common;
using 仁주문자.For주문자.Command;
using MediationAPIGateWay.Repository;
using MediationAPIGateWay.Service;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command.공동주문
{
    public class Create공동주문CommandHandler : IRequestHandler<Create공동주문Command, Unit>
    {
        private readonly 공동주문Repository _공동주문Repository;
        private readonly 전자서명Service _전자서명Service;
        private readonly I결제Service _결제Service;

        public Create공동주문CommandHandler(공동주문Repository 공동주문Repository, 전자서명Service 전자서명Service, I결제Service 결제Service)
        {
            _공동주문Repository = 공동주문Repository;
            _전자서명Service = 전자서명Service;
            _결제Service = 결제Service;
        }

        public async Task<Unit> Handle(Create공동주문Command command, CancellationToken cancellationToken)
        {
            // 1. 사용자 조회
            var 사용자 = await _공동주문Repository.GetUserAsync(command.사용자Id);
            if (사용자 == null)
            {
                throw new ApplicationException("사용자가 존재하지 않습니다.");
            }

            // 2. 주문자 정보를 JSON에서 역직렬화
            var 주문자 = JsonConvert.DeserializeObject<주문자>(사용자.주문자Json);
            if (주문자 == null)
            {
                throw new ApplicationException("주문자 정보가 유효하지 않습니다.");
            }

            // 3. 전자서명 검증
            if (!_전자서명Service.VerifySignature(command, 사용자.공개키인증서))
            {
                throw new ApplicationException("전자서명 검증 실패");
            }

            // 4. 결제 검증
            var 결제성공 = await _결제Service.결제검증(command.결제요청Dto);
            if (!결제성공)
            {
                throw new ApplicationException("결제 실패");
            }

            // 5. 공동 주문 상품 추가
            _공동주문Repository.AddOrderToUser(주문자, command);

            // 6. MongoDB 및 RDBMS 저장
            await _공동주문Repository.SaveUserAsync(사용자, 주문자);

            return Unit.Value;
        }
    }

}
