using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁매칭자.Command;
using 仁매칭자.Dto;

namespace MediationAPIGateWay.Controller.매칭
{
    [ApiController]
    [Route("api/[controller]")]
    public class 매칭Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        public 매칭Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("스킨십선호")]
        public async Task<IActionResult> Create스킨십선호([FromBody] 스킨십선호Dto dto)
        {
            var command = new Create스킨십선호Command(dto);
            await _mediator.Send(command);
            return Ok("스킨십 선호 정보가 저장되었습니다.");
        }
    }
}
