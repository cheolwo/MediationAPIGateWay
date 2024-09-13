using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁도서관련자.Command;
using 仁도서관련자.Query;

namespace MediationAPIGateWay.Controller.도서
{
    [ApiController]
    [Route("api/[controller]")]
    public class 책Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 책Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 도서 추가
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddBook([FromBody] Create도서Command command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok("책이 성공적으로 추가되었습니다.");
            }
            return BadRequest("책 추가에 실패했습니다.");
        }

        // 도서 검색
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string keyword)
        {
            var query = new Search도서Query(keyword);
            var books = await _mediator.Send(query);
            return Ok(books);
        }
    }
}
