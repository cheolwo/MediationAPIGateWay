using kakaoInfa.카카오결제;
using 카카오결제승인Request = kakaoInfa.카카오결제.RequestModel.결제승인Request;
using 토스결제승인Request = TossInfa.토스결제.RequestModel.결제승인Request;
using 네이버Infra.네이버결제;
using TossInfa.토스결제;
using 仁Infra;
using 결제Infra;
using Microsoft.EntityFrameworkCore;

namespace 仁Common
{
    public interface I결제Service
    {
        Task<bool> 결제검증(결제요청Dto request);
    }

    public class 결제Service : I결제Service
    {
        private readonly 네이버결제APIService _네이버결제APIService;
        private readonly 카카오결제APIService _카카오결제APIService;
        private readonly 토스결제APIService _토스결제APIService;
        private readonly 결제DbContext _결제DbContext; // 결제 DbContext DI

        public 결제Service(
            네이버결제APIService 네이버결제APIService,
            카카오결제APIService 카카오결제APIService,
            토스결제APIService 토스결제APIService,
            결제DbContext 결제DbContext) // 결제DbContext DI
        {
            _네이버결제APIService = 네이버결제APIService;
            _카카오결제APIService = 카카오결제APIService;
            _토스결제APIService = 토스결제APIService;
            _결제DbContext = 결제DbContext;
        }

        public async Task<bool> 결제검증(결제요청Dto request)
        {
            bool 결제성공 = false;

            // 결제 방법에 따른 분기 처리
            switch (request.결제방법)
            {
                case "카카오페이":
                    결제성공 = await 검증카카오페이(request);
                    break;
                case "네이버페이":
                    결제성공 = await 검증네이버페이(request);
                    break;
                case "토스페이":
                    결제성공 = await 검증토스페이(request);
                    break;
                default:
                    throw new ApplicationException("지원되지 않는 결제 방법입니다.");
            }

            // 결제가 성공했을 경우 결제 정보를 결제DB에 저장
            if (결제성공)
            {
                // 거래 번호가 중복되지 않았는지 확인
                var existingPayment = await _결제DbContext.결제내역들
                    .FirstOrDefaultAsync(p => p.거래번호 == request.결제고유번호);

                if (existingPayment == null)
                {
                    // 결제 내역 생성 및 저장
                    var 결제내역 = new 결제내역
                    {
                        사용자Id = request.사용자Id,
                        결제방법 = request.결제방법,
                        결제금액 = request.결제금액,
                        거래번호 = request.결제고유번호,
                        결제일시 = DateTime.UtcNow,
                        결제상태 = "승인" // 결제 성공 상태
                    };

                    _결제DbContext.결제내역들.Add(결제내역);
                    await _결제DbContext.SaveChangesAsync();
                }
            }

            return 결제성공;
        }

        // 카카오페이 결제 확인 로직
        private async Task<bool> 검증카카오페이(결제요청Dto request)
        {
            var 결제승인Request = new 카카오결제승인Request
            {
                Tid = request.결제고유번호,
            };

            var response = await _카카오결제APIService.결제승인Async(결제승인Request);

            if (response != null && response.ApprovedAt != null)
            {
                return true; // 카카오페이 결제가 승인된 경우
            }

            return false;
        }

        // 네이버페이 결제 확인 로직
        private async Task<bool> 검증네이버페이(결제요청Dto request)
        {
            var response = await _네이버결제APIService.결제승인Async(request.결제고유번호);

            if (response != null && response.Code == "Success" && response.Body.Detail.AdmissionState == "SUCCESS")
            {
                return true; // 네이버페이 결제가 성공적으로 승인된 경우
            }

            return false;
        }

        // 토스페이 결제 확인 로직
        private async Task<bool> 검증토스페이(결제요청Dto request)
        {
            var 결제승인Request = new 토스결제승인Request
            {
                PayToken = request.결제고유번호,
                ApiKey = "your-toss-api-key"
            };

            var response = await _토스결제APIService.결제승인Async(결제승인Request);

            if (response.IsSuccessStatusCode)
            {
                return true; // 토스페이 결제가 성공적으로 승인된 경우
            }

            return false;
        }
    }
}