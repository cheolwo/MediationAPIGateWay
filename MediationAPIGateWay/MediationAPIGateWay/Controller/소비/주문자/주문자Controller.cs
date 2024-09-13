using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁주문자.For주문자.Command;
using 仁주문자.For주문자.Query;

namespace MediationAPIGateWay.Controller.소비.주문자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 주문자Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 주문자Controller(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Create구매상품후기")]
        public async Task<IActionResult> Create구매상품후기([FromBody] Create구매상품후기Command command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("구매상품 후기가 성공적으로 등록되었습니다.");
            }
            catch (Exception ex)
            {
                return BadRequest($"구매상품 후기 등록 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        [HttpPost("생산자목록조회")]
        public async Task<IActionResult> Get생산자목록([FromBody] Get생산자목록Query command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 상품 목록 조회
        [HttpPost("상품목록조회")]
        public async Task<IActionResult> Get상품목록([FromBody] Get상품목록Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 상품 주문
        [HttpPost("상품주문")]
        public async Task<IActionResult> Create상품주문([FromBody] Create상품주문Command command)
        {
            await _mediator.Send(command);
            return Ok("주문이 성공적으로 처리되었습니다.");
        }

        // 생산자 목록 조회
        // 주문 상태 조회
        [HttpPost("주문상태조회")]
        public async Task<IActionResult> Get주문상태([FromBody] Get주문상태Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 주문자 후기 작성
        [HttpPost("후기작성")]
        public async Task<IActionResult> Create주문자후기([FromBody] Create주문자후기Command command)
        {
            await _mediator.Send(command);
            return Ok("후기가 성공적으로 작성되었습니다.");
        }

        // 주문 내역 조회
        [HttpPost("주문내역조회")]
        public async Task<IActionResult> Get주문내역([FromBody] Get주문내역Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 생산자 문의
        [HttpPost("생산자문의")]
        public async Task<IActionResult> Create생산자문의([FromBody] Create생산자문의Command command)
        {
            await _mediator.Send(command);
            return Ok("문의가 성공적으로 전달되었습니다.");
        }
    }
}