using MediatR;
using Microsoft.AspNetCore.Mvc;
using 국토교통부_공공데이터Common.개발사용료_정보제공_서비스;
using 국토교통부_공공데이터Common.에너지사용정보_정보서비스.RequestModel;
using 국토교통부_공공데이터Common.공용관리비_정보제공서비스;
using 국토교통부_공공데이터Common.장기수선충당금_정보서비스;
using MediationAPIGateWay.Handlr.소비.공동주택단지.Request;

namespace MediationAPIGateWay.Controller.소비.공동주택InfraController
{
    [Route("api/[controller]")]
    [ApiController]
    public class 비용정보수집Controller : ControllerBase
    {
        private readonly IMediator _Mediator;
        private readonly ILogger<비용정보수집Controller> _Logger;

        public 비용정보수집Controller(IMediator mediator, ILogger<비용정보수집Controller> logger)
        {
            _Mediator = mediator;
            _Logger = logger;
        }

        [HttpPost("비용수집")]
        public async Task<IActionResult> Post비용정보([FromBody] 비용정보수집Request request)
        {
            try
            {
                // 각 핸들러에 전달할 요청 생성
                var 개별사용료Request = new 개별사용료정보제공Request
                {
                    kaptCode = request.kaptCode,
                    searchDate = request.date
                };

                var 공용관리비Request = new 공용관리비Request
                {
                    kaptCode = request.kaptCode,
                    searchDate = request.date
                };

                var 에너지사용량Request = new 단지에너지사용량Request
                {
                    kaptCode = request.kaptCode,
                    searchDate = request.date
                };

                var 장기수선충당금Request = new 장기수선충당금Request
                {
                    kaptCode = request.kaptCode,
                    searchDate = request.date
                };

                // 각 핸들러 실행
                await _Mediator.Send(개별사용료Request);
                await _Mediator.Send(공용관리비Request);
                await _Mediator.Send(에너지사용량Request);
                await _Mediator.Send(장기수선충당금Request);

                return Ok("비용 정보 수집이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                _Logger.LogError($"비용 정보 수집 중 오류 발생: {ex.Message}");
                return StatusCode(500, "서버 내부 오류가 발생했습니다.");
            }
        }
    }
}
