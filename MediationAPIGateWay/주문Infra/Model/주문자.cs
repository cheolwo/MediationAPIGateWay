using Common.Model;

namespace 주문Infra.Model
{
    // 주문 상태 Enum
    public enum 주문상태
    {
        미확인,
        확인,
        할당,
        피킹,
        포장,
        출고
    }

    // 집단주문 엔티티 (주문자집단)
    public class 주문자집단 : Entity
    {
        public string 집단코드 { get; set; }
        public string 단지명 { get; set; }
        public string 법정동코드 { get; set; }
        public string 법정동주소 { get; set; }
        public decimal 보증금 { get; set; }
        public string 대표주문자Id { get; set; }
        public List<주문자> 주문자들 { get; set; } = new List<주문자>(); // 소속된 주문자들
        public List<주문상품> 주문상품목록 { get; set; } = new List<주문상품>(); // 주문 상품 목록
    }

    // 개별주문 상품 엔티티 (주문상품)
    public class 주문상품 : Entity
    {
        public string 상품코드 { get; set; }
        public string 상품명 { get; set; }
        public string 설명 { get; set; }
        public decimal 가격 { get; set; }
        public 주문상태 주문상태 { get; set; }  // 주문상태를 enum으로 변경

        // Foreign key for 생산자
        public string 생산자Id { get; set; }
        public string 판매자Id { get; set; }

        // 주문자 정보 (개별주문 상품에만 해당)
        public string 주문자Id { get; set; }
        public 주문자 주문자 { get; set; }

        // 주문자 집단 정보
        public string 주문자집단Id { get; set; }
        public 주문자집단 주문자집단 { get; set; }

        // 할인된 금액 (할인된 금액만 기록)
        public decimal 할인금액 { get; set; }

        // 최종 결제 금액 (할인 적용된 가격)
        public decimal 최종결제금액 { get; set; }
    }

    // 주문자 엔티티 (개인주문자)
    public class 주문자 : Entity
    {
        public string 이름 { get; set; } // 주문자 이름
        public string 연락처 { get; set; } // 주문자 연락처
        public List<주문자집단> 주문자집단목록 { get; set; } = new List<주문자집단>(); // 소속된 집단 목록
        public List<주문상품> 주문상품목록 { get; set; } = new List<주문상품>(); // 주문한 상품 목록
    }
}
