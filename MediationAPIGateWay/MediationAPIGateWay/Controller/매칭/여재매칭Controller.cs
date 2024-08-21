using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.매칭
{
    [ApiController]
    [Route("api/[controller]")]
    public class 여자매칭Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 여자매칭Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 매칭 선호 정보 입력
        [HttpPost("매칭선호입력")]
        public async Task<IActionResult> 매칭선호입력([FromBody] 매칭선호정보Dto dto)
        {
            var command = new Create여자매칭선호Command(dto);
            await _mediator.Send(command);
            return Ok("매칭 선호 정보가 저장되었습니다.");
        }

        // 매칭 결과 확인
        [HttpGet("매칭결과/{사용자Id}")]
        public async Task<IActionResult> 매칭결과(string 사용자Id)
        {
            var query = new Get매칭결과Query(사용자Id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 매칭 피드백 제출
        [HttpPost("매칭피드백")]
        public async Task<IActionResult> 매칭피드백([FromBody] 매칭피드백Dto dto)
        {
            var command = new Submit매칭피드백Command(dto);
            await _mediator.Send(command);
            return Ok("매칭 피드백이 제출되었습니다.");
        }

        // 자리 교환 승인
        [HttpPost("자리교환승인")]
        public async Task<IActionResult> 자리교환승인([FromBody] 자리교환승인Dto dto)
        {
            var command = new Create자리교환승인Command(dto);
            await _mediator.Send(command);
            return Ok("자리 교환 승인이 완료되었습니다.");
        }
    }

}
