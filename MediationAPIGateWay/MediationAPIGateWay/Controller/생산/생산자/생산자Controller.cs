using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediationAPIGateWay.Controller.생산.생산자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 생산자Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 생산자Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

    }
}
