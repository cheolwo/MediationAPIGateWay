using MediatR;
using 仁매칭infra;

namespace 仁생산자.Query
{
    public class Get근로인력매칭By생산자IdQuery : IRequest<List<근로인력매칭>>
    {
        public string 생산자Id { get; set; }
    }
}
