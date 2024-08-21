using Microsoft.AspNetCore.Mvc;
using 인력관리업체.Command;
using 仁Common.Command;

namespace CommandServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class 인력관리업체Controller : ControllerBase
    {
        private readonly ICommandQueue _commandQueue;

        public 인력관리업체Controller(ICommandQueue commandQueue)
        {
            _commandQueue = commandQueue;
        }

        // 모집 공고 생성 엔드포인트
        [HttpPost("CreateJobPosting")]
        public async Task<IActionResult> CreateJobPosting([FromBody] Create모집공고Command command)
        {
            // Command를 큐에 추가
            await _commandQueue.EnqueueAsync(command);
            return Ok("모집 공고가 큐에 추가되었습니다.");
        }
    }
}
