using MediatR;
using 근로Infra;


namespace 仁생산자.Query
{
    public class Get가용인력Query : IRequest<List<근로신청>>
    {
        public string 지역 { get; set; } // 생산자의 지역 정보 (필수)
        public double 생산자위도 { get; set; } // 생산자의 위도 정보
        public double 생산자경도 { get; set; } // 생산자의 경도 정보
    }
}
