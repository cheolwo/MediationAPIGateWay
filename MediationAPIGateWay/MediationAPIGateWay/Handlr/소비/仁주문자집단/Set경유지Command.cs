using MediatR;

namespace MediationAPIGateWay.Handlr.소비.仁주문자집단
{
    public class Set경유지Command : IRequest<Unit>
    {
        public string 주문자집단Id { get; set; } // 주문자 집단 ID
        public string 배송자Id { get; set; } // 배송자 ID
        public List<string> 경유지목록 { get; set; } // 경유지 목록 (생산자 출하 장소 목록)

        // 추가로, 경유지 설정에 대한 상세 정보가 필요하다면 아래와 같은 속성을 추가할 수 있음.
        public DateTime 배송일자 { get; set; } // 배송일자
        public string 최적화옵션 { get; set; } // 탐색 옵션 (예: 빠른 길, 편한 길 등)

        public Set경유지Command(string 주문자집단Id, string 배송자Id, List<string> 경유지목록, DateTime 배송일자, string 최적화옵션)
        {
            this.주문자집단Id = 주문자집단Id;
            this.배송자Id = 배송자Id;
            this.경유지목록 = 경유지목록;
            this.배송일자 = 배송일자;
            this.최적화옵션 = 최적화옵션;
        }
    }
}
