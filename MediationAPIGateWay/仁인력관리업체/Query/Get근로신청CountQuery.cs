using MediatR;

namespace 仁농협.Query
{
    // 근로신청 수 조회를 위한 Query
    public class Get근로신청CountQuery : IRequest<int>
    {
        public int 근무지Id { get; set; } // 근무지 ID
    }
}
