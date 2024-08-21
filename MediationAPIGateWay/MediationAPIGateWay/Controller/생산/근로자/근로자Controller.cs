using MediationAPIGateWay.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁Common.근로자;
using 仁근로자;
using 仁근로자.Command;

namespace MediationAPIGateWay.Controller.생산.근로자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 근로자Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly 셔틀버스서비스 _셔틀버스서비스;

        public 근로자Controller(IMediator mediator, 셔틀버스서비스 셔틀버스서비스)
        {
            _mediator = mediator;
            _셔틀버스서비스 = 셔틀버스서비스;
        }

        // 관리자 전용 고객 정보 조회 API (모든 데이터 제공)
        [HttpGet("고객조회/{지하철역Id}")]
        public async Task<IActionResult> Get고객정보For관리자(int 지하철역Id)
        {
            var 고객정보들 = await _셔틀버스서비스.Get고객정보For관리자(지하철역Id);
            return Ok(고객정보들);
        }
        [HttpPost("Create근로후기")]
        public async Task<IActionResult> Create근로후기([FromBody] Create근로후기Command command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("근로 후기가 성공적으로 등록되었습니다.");
            }
            catch (System.Exception ex)
            {
                return BadRequest($"근로 후기 등록 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        // 근로신청과 결제를 함께 처리하는 API
        // 근로 신청
        [HttpPost("근로신청")]
        public async Task<IActionResult> Create근로신청([FromBody] Create근로신청Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        // 매칭 결과 조회
        [HttpPost("매칭결과조회")]
        public async Task<IActionResult> Get매칭결과([FromBody] Get매칭결과Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 후기 작성
        [HttpPost("후기작성")]
        public async Task<IActionResult> Create후기작성([FromBody] Create후기작성Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
