using System.ComponentModel.DataAnnotations;

namespace 주문Common.Model
{
    // 주문 상태 Enum
    public enum 주문Status
    {
        미확인,
        확인,
        할당,
        피킹,
        포장,
        출고
    }

    // 집단주문 엔티티 (주문자집단)
    public class 주문자집단
    {
        [Key]
        public string 집단코드 { get; set; }
        public string 단지명 { get; set; }
        public string 법정동코드 { get; set; }
        public string 법정동주소 { get; set; }
        public List<주문자> 주문자들 { get; set; } // 소속된 주문자들
        public List<공동주문상품> 공동주문상품들 { get; set; } // 공동주문 상품들
    }

    // 공동주문 상품 엔티티 (공동주문상품)
    public class 공동주문상품
    {
        [Key]
        public string 상품코드 { get; set; }
        public string 상품명 { get; set; }
        public string 설명 { get; set; }
        public decimal 가격 { get; set; }
        public DateTime 발의일자 { get; set; }
        public 주문Status 주문상태 { get; set; }

        // Foreign key for 주문자집단
        public string 집단코드 { get; set; }
        public 주문자집단 주문자집단 { get; set; }

        // Foreign key for 생산자
        public string 생산자Id { get; set; }
        public 생산자 생산자 { get; set; }
    }

    // 개별주문 상품 엔티티 (개별주문상품)
    public class 개별주문상품
    {
        [Key]
        public string 상품코드 { get; set; }
        public string 상품명 { get; set; }
        public string 설명 { get; set; }
        public decimal 가격 { get; set; }
        public 주문Status 주문상태 { get; set; }

        // Foreign key for 생산자
        public string 생산자Id { get; set; }
        public 생산자 생산자 { get; set; }

        // 주문자 정보 (개별주문 상품에만 해당)
        public string 주문자Id { get; set; }
        public 주문자 주문자 { get; set; }
    }

    // 주문자 엔티티 (개인주문자)
    public class 주문자
    {
        [Key]
        public string 주문자Id { get; set; }
        public string 이름 { get; set; } // 주문자 이름
        public string 연락처 { get; set; } // 주문자 연락처

        // 주문자가 주문한 상품들
        public List<주문상품> 주문상품들 { get; set; }
    }

    // 개인주문 상품 엔티티 (주문상품)
    public class 주문상품
    {
        [Key]
        public string 상품코드 { get; set; }
        public string 상품명 { get; set; }
        public decimal 가격 { get; set; }

        public string 주문자Id { get; set; }
        public 주문자 주문자 { get; set; }

        public 주문Status 주문상태 { get; set; }
    }

    // 생산자 엔티티
    public class 생산자
    {
        [Key]
        public string 생산자Id { get; set; }
        public string 이름 { get; set; }
        public string 연락처 { get; set; }
        public string 주소 { get; set; }

        // 생산자가 판매하는 공동주문 상품 목록
        public List<공동주문상품> 공동주문상품들 { get; set; }

        // 생산자가 판매하는 개별주문 상품 목록
        public List<개별주문상품> 개별주문상품들 { get; set; }
    }

    // 주문 상태 엔티티 (상태 추적)
    public class 주문상태
    {
        [Key]
        public string 주문상태Id { get; set; }
        public 주문Status Status { get; set; }  // 주문 상태 (Enum)
        public string 입고Id { get; set; }
        public string 계약Id { get; set; }
        public string 적재Id { get; set; }
        public string 마켓Id { get; set; }
        public string 출고Id { get; set; }
        public string 결재Id { get; set; }
    }
}

