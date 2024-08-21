using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common
{
    public interface IPaymentService
    {
        Task<bool> VerifyPayment(결제요청Dto request);
    }

    public class PaymentService : IPaymentService
    {
        public async Task<bool> VerifyPayment(결제요청Dto request)
        {
            // 결제 방법에 따른 분기 처리
            switch (request.결제방법)
            {
                case "카카오페이":
                    return await VerifyKakaoPay(request);
                case "네이버페이":
                    return await VerifyNaverPay(request);
                case "토스페이":
                    return await VerifyTossPay(request);
                default:
                    throw new ApplicationException("지원되지 않는 결제 방법입니다.");
            }
        }

        // 카카오페이 결제 확인 로직
        private async Task<bool> VerifyKakaoPay(결제요청Dto request)
        {
            // 카카오페이의 API를 호출하여 결제 상태를 확인
            // 실제 API 호출 로직을 여기에 추가
            // 예시: HttpClient로 결제 고유번호를 이용하여 결제 상태 확인
            // API 호출 결과에 따라 true 또는 false 반환
            return true;
        }

        // 네이버페이 결제 확인 로직
        private async Task<bool> VerifyNaverPay(결제요청Dto request)
        {
            // 네이버페이 API를 호출하여 결제 상태 확인
            return true;
        }

        // 토스페이 결제 확인 로직
        private async Task<bool> VerifyTossPay(결제요청Dto request)
        {
            // 토스페이 API를 호출하여 결제 상태 확인
            return true;
        }
    }
    public class 결제요청Dto
    {
        public string 결제고유번호 { get; set; }  // 페이사에서 제공하는 결제 고유번호
        public string 거래번호 { get; set; }     // 각 거래에 대한 고유 거래번호
        public string 결제방법 { get; set; }      // 결제 방법 (카카오페이, 네이버페이, 토스페이 등)
        public decimal 결제금액 { get; set; }     // 결제 금액
        public DateTime 결제일시 { get; set; }    // 결제가 이루어진 일시
    }

}
