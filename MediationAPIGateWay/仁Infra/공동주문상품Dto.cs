
namespace 仁Infra
{
    // 공동주문에 포함된 상품에 대한 DTO 클래스
    // 공동주문 상품에 대한 DTO 클래스
    public class 공동주문상품Dto
    {
        public string 상품코드 { get; set; } // 상품 코드
        public string 상품명 { get; set; } // 상품명
        public string 설명 { get; set; } // 상품 설명
        public decimal 가격 { get; set; } // 상품 가격
        public DateTime 발의일자 { get; set; } // 발의 일자
        public string 주문상태 { get; set; } // 주문 상태

        // Foreign key for 주문자집단
        public string 집단코드 { get; set; } // 주문자 집단 코드

        public string 생산자Id { get; set; } // 생산자 ID

        // 기본 생성자
        public 공동주문상품Dto() { }

        // 모든 필드를 초기화하는 생성자
        public 공동주문상품Dto(string 상품코드, string 상품명, string 설명, decimal 가격, DateTime 발의일자, 주문Status 주문상태, string 집단코드, string 생산자Id)
        {
            this.상품코드 = 상품코드;
            this.상품명 = 상품명;
            this.설명 = 설명;
            this.가격 = 가격;
            this.발의일자 = 발의일자;
            this.주문상태 = 주문상태;
            this.집단코드 = 집단코드;
            this.생산자Id = 생산자Id;
        }
    }


}
