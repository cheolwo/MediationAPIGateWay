using MediatR;
using Microsoft.AspNetCore.Mvc;
using 국토교통부_공공데이터Common.공동주택_기본정보_제공서비스.RequestModel;
using 국토교통부_공공데이터Common.공동주택_단지_목록제공_서비스;

namespace MediationAPIGateWay.Controller.소비.공동주택InfraController
{
    [Route("api/[controller]")]
    [ApiController]
    public class 공동주택정보수집Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<공동주택정보수집Controller> _logger;

        public 공동주택정보수집Controller(IMediator mediator,
            ILogger<공동주택정보수집Controller> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("CollectAllComplexes")]
        public async Task<IActionResult> CollectAllComplexes()
        {
            try
            {
                await _mediator.Send(new 공동주택단지Request());
                return Ok("All complexes have been successfully collected and updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while collecting all complexes: " +
                    $"{ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("CollectComplexDetails/{kaptCode}")]
        public async Task<IActionResult> CollectComplexDetails(string kaptCode)
        {
            try
            {
                await _mediator.Send(new 공동주택상세정보Request { kaptCode = kaptCode });
                return Ok($"Details for complex {kaptCode} have been successfully " +
                    $"collected and updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while collecting details for complex " +
                    $"{kaptCode}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
