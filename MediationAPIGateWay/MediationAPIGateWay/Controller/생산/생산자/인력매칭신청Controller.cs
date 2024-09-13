using MediatR;
using Microsoft.AspNetCore.Mvc;
using 仁Infra.생산자.DTO;
using 仁생산자.Query;

namespace MediationAPIGateWay.Controller.생산.생산자
{
    [ApiController]
    [Route("api/[controller]")]
    public class 인력매칭신청Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public 인력매칭신청Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<인력매칭신청DTO>>> Get매칭신청List([FromQuery] string 생산자Id)
        {
            var query = new Get인력매칭신청ListQuery { 생산자Id = 생산자Id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
