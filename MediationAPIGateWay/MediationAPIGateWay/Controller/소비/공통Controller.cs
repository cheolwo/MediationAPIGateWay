using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.소비
{
    [ApiController]
    [Route("api/[controller]")]
    public class 공통Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 공통Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("email-notification")]
        public async Task<IActionResult> SendEmailNotification([FromBody] SendEmailNotificationCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("sms-notification")]
        public async Task<IActionResult> SendSmsNotification([FromBody] SendSmsNotificationCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
