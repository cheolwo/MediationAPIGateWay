using Common.Model;

namespace 셔틀Infra.Model
{
    // 고객 정보를 저장하는 엔티티
    public class 고객정보 : Entity
    {
        public string 이름 { get; set; }
        public string 성별 { get; set; }
        public string 전화번호 { get; set; }
        public int 셔틀마차배차Id { get; set; }
        public 셔틀마차배차 셔틀마차배차 { get; set; }
    }

    public class 셔틀마차배차 : Entity
    {
        public int 마을Id { get; set; }
        public 마을 마을 { get; set; }
        public int 셔틀마차Id { get; set; }
        public 셔틀마차 셔틀마차 { get; set; }
        public bool 이성매칭 { get; set; }
        public DateTime 출발시간 { get; set; }
        public DateTime 배차일자 { get; set; }
        public List<고객정보> 고객들 { get; set; }
        public List<마을> 휴게마을목록 { get; set; }
    }

    public class 마을 : Entity
    {
        public string 마을명 { get; set; }
        public List<수정구> 수정구목록 { get; set; }
    }
    public class 수정구 : Entity
    {
        public int 포집마나량 { get; set; }
    }

    public class 셔틀마차 : Entity
    {
        public string 회사 { get; set; }
        public string 마차번호 { get; set; }
        public string 마차기사명 { get; set; }
        public List<수정구> 수정구목록 { get; set; }
    }
}
