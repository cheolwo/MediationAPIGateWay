using MediatR;
using 생산Infra.Model;

namespace 仁근로자
{
    public class Get농협정보With근무지Query : IRequest<농협>
    {
        public int 농협Id { get; set; } // 농협 ID
    }
    public class Get근무지후기Query : IRequest<List<후기>>
    {
        public int 근무지Id { get; set; } // 근무지 ID
    }
}
