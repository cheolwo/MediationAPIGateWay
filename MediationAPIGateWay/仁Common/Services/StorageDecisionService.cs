using 결제Infra;
using 근로Infra;
using 이성Infra;
using 배송Infra.Model;
using 생산Infra;
using 셔틀Infra;
using 주문Infra;
using 창고Common.Model;
using 판매Infra;
using 仁Common.Command;

namespace 仁Common.Services
{
    public interface IStorageDecisionService
    {
        Type DecideDbContext(IUserCommand command);
    }

    public class StorageDecisionService : IStorageDecisionService
    {
        public Type DecideDbContext(IUserCommand command)
        {
            string context = command.Context.ToLower(); // 명시된 Context 사용

            switch (context)
            {
                case "주문":
                    return typeof(주문DbContext);
                case "결제":
                    return typeof(결제DbContext);
                case "근로":
                    return typeof(근로DbContext);
                case "배송":
                    return typeof(배송DbContext);
                case "생산":
                    return typeof(생산DbContext);
                case "셔틀":
                    return typeof(셔틀DbContext);
                case "이성":
                    return typeof(이성DbContext); ;
                case "창고":
                    return typeof(창고DbContext);
                case "판매":
                    return typeof(판매DbContext);
                default:
                    throw new ArgumentException("Unsupported context");
            }
        }
    }
}
