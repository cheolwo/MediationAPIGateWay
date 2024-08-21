using CommandServer.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁Common.근로자;

namespace CommandServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class 근로자Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PaymentService _paymentService;
        private readonly MatchingService _matchingService;

        public 근로자Controller(IMediator mediator, PaymentService paymentService, MatchingService matchingService)
        {
            _mediator = mediator;
            _paymentService = paymentService;
            _matchingService = matchingService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        // 남성 근로자 신청 API
        [HttpPost("CreateMaleJobApplication")]
        public async Task<IActionResult> CreateMaleJobApplication([FromBody] Create근로신청Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("입력 데이터가 유효하지 않습니다.");
            }

            command.성별 = "남성"; // 남성 근로 신청임을 명시

            // 연하 매칭 시 결제 필요 여부 확인 (남성)
            if (command.연상선호 == false && !command.결제완료)
            {
                bool paymentSuccess = _paymentService.ProcessPayment(command, 10000); // 예: 10,000원 결제
                if (!paymentSuccess)
                {
                    return BadRequest("결제가 실패하였습니다.");
                }
            }

            // 근로 신청 처리 (MediatR을 통해 처리)
            await _mediator.Send(command);

            // 매칭 로직 호출
            var 근로자들 = new List<Worker>(); // 근로자 리스트는 외부에서 주입 받음
            var matchedWorker = _matchingService.PerformMatching(근로자들, new Worker
            {
                WorkerId = command.근로자Id,
                Age = command.나이,
                Gender = command.성별,
                IsOlderWomanPreferred = command.연상선호
            });

            if (matchedWorker != null)
            {
                return Ok($"{command.근로자Id}님의 매칭이 완료되었습니다: {matchedWorker.WorkerId}");
            }

            return Ok("남성 근로 신청이 완료되었습니다.");
        }

        // 여성 근로자 신청 API
        [HttpPost("CreateFemaleJobApplication")]
        public async Task<IActionResult> CreateFemaleJobApplication([FromBody] Create근로신청Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("입력 데이터가 유효하지 않습니다.");
            }

            command.성별 = "여성"; // 여성 근로 신청임을 명시

            // 연하 매칭 시 결제 필요 여부 확인 (여성)
            if (command.연상선호 == false && !command.결제완료)
            {
                bool paymentSuccess = _paymentService.ProcessPayment(command, 10000); // 예: 10,000원 결제
                if (!paymentSuccess)
                {
                    return BadRequest("결제가 실패하였습니다.");
                }
            }

            // 근로 신청 처리 (MediatR을 통해 처리)
            await _mediator.Send(command);

            // 매칭 로직 호출
            var 근로자들 = new List<Worker>(); // 근로자 리스트는 외부에서 주입 받음
            var matchedWorker = _matchingService.PerformMatching(근로자들, new Worker
            {
                WorkerId = command.근로자Id,
                Age = command.나이,
                Gender = command.성별,
                IsOlderWomanPreferred = command.연상선호
            });

            if (matchedWorker != null)
            {
                return Ok($"{command.근로자Id}님의 매칭이 완료되었습니다: {matchedWorker.WorkerId}");
            }

            return Ok("여성 근로 신청이 완료되었습니다.");
        }
    }

}
