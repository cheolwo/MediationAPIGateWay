using MediatR;
using 仁매칭infra;
using 仁생산자.Query;

namespace MediationAPIGateWay.Handlr.생산.생산자.Query
{
    public class Get근로인력매칭By생산자IdQueryHandler : IRequestHandler<Get근로인력매칭By생산자IdQuery, List<근로인력매칭>>
    {
        private readonly 매칭Service _매칭Service;

        public Get근로인력매칭By생산자IdQueryHandler(매칭Service 매칭Service)
        {
            _매칭Service = 매칭Service;
        }

        public async Task<List<근로인력매칭>> Handle(Get근로인력매칭By생산자IdQuery request, CancellationToken cancellationToken)
        {
            return await _매칭Service.Get근로인력매칭By생산자IdAsync(request.생산자Id);
        }
    }
}
