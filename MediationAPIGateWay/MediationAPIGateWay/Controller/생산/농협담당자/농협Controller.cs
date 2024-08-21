using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁농협.Command;
using 仁농협.Dto;

namespace MediationAPIGateWay.Controller.생산.농협담당자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 농협Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 농협Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 근로신청 승인 API
        [HttpPost("근로신청승인")]
        public async Task<IActionResult> Approve근로신청([FromBody] 근로신청승인요청Dto request)
        {
            try
            {
                // 승인 처리
                await _mediator.Send(new Approve근로신청Command(request.근로신청Id, request.농협담당자Id));
                return Ok("근로신청이 성공적으로 승인되었습니다.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // 복수 근로신청 승인 API
        [HttpPost("근로신청복수승인")]
        public async Task<IActionResult> Approve근로신청들([FromBody] 근로신청복수승인요청Dto request)
        {
            try
            {
                // 복수 근로신청 승인 처리
                await _mediator.Send(new Approve근로신청들Command(request.근로신청Ids, request.농협담당자Id));
                return Ok("근로신청들이 성공적으로 승인되었습니다.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
