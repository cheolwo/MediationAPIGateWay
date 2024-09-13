using MediatR;
using Newtonsoft.Json;
using 사용자Infra;
using 仁Common;
using 仁주문자.For주문자.Command;
using MediationAPIGateWay.Repository;
using 주문Infra.Model;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command.개별주문
{
    public class Create개별주문CommandHandler : IRequestHandler<Create개별주문Command, Unit>
    {
        private readonly 개별주문Repository _개별주문Repository;
        private readonly I결제Service _결제Service;

        public Create개별주문CommandHandler(개별주문Repository 개별주문Repository, I결제Service 결제Service)
        {
            _개별주문Repository = 개별주문Repository;
            _결제Service = 결제Service;
        }

        public async Task<Unit> Handle(Create개별주문Command command, CancellationToken cancellationToken)
        {
            // 1. MongoDB에서 사용자 정보 조회
            var 사용자 = await _개별주문Repository.GetUserAsync(command.사용자Id);
            if (사용자 == null)
            {
                throw new ApplicationException("사용자가 존재하지 않습니다.");
            }

            // 2. 주문자 정보를 JSON에서 역직렬화
            var 결제주문자 = JsonConvert.DeserializeObject<주문자>(사용자.주문자Json);
            if (결제주문자 == null)
            {
                throw new ApplicationException("주문자 정보가 유효하지 않습니다.");
            }

            // 3. 결제 검증 로직 추가
            var 결제성공 = await _결제Service.결제검증(command.결제요청Dto);
            if (!결제성공)
            {
                throw new ApplicationException("결제 실패");
            }

            // 4. 새로운 개별 주문 상품 생성
            var newOrderProduct = new 주문상품
            {
                상품코드 = command.상품Id.ToString(),
                상품명 = command.상품명,  // Command에서 전달된 상품명 사용
                가격 = command.가격,      // Command에서 전달된 가격 사용
                주문자Id = command.사용자Id,
                주문상태 = 주문상태.미확인
            };

            // 5. 주문자 객체에 개별 주문 상품 추가
            if (결제주문자.주문상품목록 == null)
            {
                결제주문자.주문상품목록 = new List<주문상품>();
            }
            결제주문자.주문상품목록.Add(newOrderProduct);

            // 6. MongoDB에서 사용자 정보 업데이트
            await _개별주문Repository.UpdateUserAsync(사용자, 결제주문자);

            // 7. RDBMS에 새로운 주문 저장
            await _개별주문Repository.SaveOrderAsync(newOrderProduct);

            return Unit.Value;
        }
    }

}
