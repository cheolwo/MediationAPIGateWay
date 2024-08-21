using MediationAPIGateWay.Service;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.생산.관리자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 관리자Controller : ControllerBase
    {
        private readonly 셔틀버스서비스 _셔틀버스서비스;

        public 관리자Controller(셔틀버스서비스 셔틀버스서비스)
        {
            _셔틀버스서비스 = 셔틀버스서비스;
        }

        // 관리자 전용 고객 정보 조회 API (모든 데이터 제공)
        [HttpGet("고객조회/{지하철역Id}")]
        public async Task<IActionResult> Get고객정보For관리자(int 지하철역Id)
        {
            var 고객정보들 = await _셔틀버스서비스.Get고객정보For관리자(지하철역Id);
            return Ok(고객정보들);
        }
    }
}
