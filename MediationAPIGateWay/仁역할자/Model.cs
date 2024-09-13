using Common.Model;

namespace 仁물류Infra
{
    public class 물류관리자 : Entity
    {
        public string Role { get; set; }
        public string 이름 { get; set; } // 집단 이름
        public List<입고기록> 입고기록목록 { get; set; } // 물류 기록들
        public List<적재기록> 적재기록목록 { get; set; } // 물류 기록들
        public List<출고기록> 출고기록목록 { get; set; } // 물류 기록들
    }
    public class 물류상품 : Entity
    {
        public string 상품명 { get; set; } // 상품 이름
        public decimal 가격 { get; set; } // 가격
        public int 수량 { get; set; } // 물류에서 다루는 수량
        public string 출발위치 { get; set; } // 출발 위치
        public string 도착위치 { get; set; } // 도착 위치
        public decimal 예상비용 { get; set; } // 예상 물류 비용
    }
    public class 입고기록 : Entity
    {
        public DateTime 입고일자 { get; set; }
        public string 위치 { get; set; } // 입고 위치
        public 물류상품 물류상품 { get; set; } // 입고된 물류상품
        public int 물류관리자Id { get; set; }
        public 물류관리자 물류관리자 { get; set; } // 물류관리자 (입고를 관리하는 관리자)
    }

    public class 적재기록 : Entity
    {
        public DateTime 적재일자 { get; set; }
        public string 위치 { get; set; } // 적재 위치
        public 물류상품 물류상품 { get; set; } // 적재된 물류상품
        public int 물류관리자Id { get; set; }
        public 물류관리자 물류관리자 { get; set; } // 물류관리자 (적재를 관리하는 관리자)
    }

    public class 출고기록 : Entity
    {
        public DateTime 출고일자 { get; set; }
        public string 위치 { get; set; } // 출고 위치
        public 물류상품 물류상품 { get; set; } // 출고된 물류상품
        public int 물류관리자Id { get; set; }
        public 물류관리자 물류관리자 { get; set; } // 물류관리자 (출고를 관리하는 관리자)
    }
    public class 물류비용예측 : Entity
    {
        public string 출발위치 { get; set; } // 출발 위치
        public string 도착위치 { get; set; } // 도착 위치
        public decimal 예상비용 { get; set; } // 예상 물류 비용
        public decimal 거리 { get; set; } // 거리 정보
    }
}
