using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.생산.생산자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 생산자Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 생산자Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 생산자 정보 조회
        [HttpPost("생산자조회")]
        public async Task<IActionResult> Get생산자정보([FromBody] Get생산자정보Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 생산자 정보 수정
        [HttpPost("생산자수정")]
        public async Task<IActionResult> Update생산자정보([FromBody] Update생산자정보Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        // 인력 매칭 신청
        [HttpPost("인력매칭신청")]
        public async Task<IActionResult> Create인력매칭([FromBody] Create인력매칭Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
