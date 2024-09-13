using MediatR;
using 仁주문자.For주문자.Command;
using 판매Infra;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Command
{
    public class Create구매상품후기CommandHandler : IRequestHandler<Create구매상품후기Command, Unit>
    {
        private readonly 판매DbContext _판매Context;

        public Create구매상품후기CommandHandler(판매DbContext 판매Context)
        {
            _판매Context = 판매Context;
        }

        public async Task<Unit> Handle(Create구매상품후기Command request, CancellationToken cancellationToken)
        {
            // Step 1: 판매상품에 대한 후기 작성
            var 판매상품후기 = new 판매Infra.Model.후기
            {
                작성자Id = request.작성자Id,
                상품상세정보Id = request.상품상세정보Id,
                내용 = request.내용,
                평점 = request.평점,
                작성일 = request.작성일,
                비공개여부 = request.비공개여부  // 비공개 여부 설정
            };

            _판매Context.후기들.Add(판매상품후기);

            // Step 2: DB 저장
            await _판매Context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
