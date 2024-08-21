using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.소비
{
    [ApiController]
    [Route("api/[controller]")]
    public class 배송자Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 배송자Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("query")]
        public async Task<IActionResult> HandleQuery([FromBody] IRequest<object> query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("command")]
        public async Task<IActionResult> HandleCommand([FromBody] IRequest command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
