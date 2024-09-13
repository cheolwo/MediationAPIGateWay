using Common.Model;

namespace 근로Infra
{
    public class 근로자 : Entity
    {
        public string 이름 { get; set; } // 사용자 이름
        public List<근로자결제> 결제정보 { get; set; }
        public List<근로신청> 근로신청들 { get; set; }
    }

    public class 근로자결제 : Entity
    {
        public 근로자 근로자 { get; set; }
        public string 근로자Id { get; set; } // 근로자 ID
        public decimal 금액 { get; set; } // 결제 금액
        public DateTime 결제일자 { get; set; } // 결제 날짜
        public DateTime 만료일자 { get; set; } // 프리미엄 서비스 만료일
    }

    public class 근로신청 : Entity
    {
        public 근로자 근로자 { get; set; }
        public string 근로자Id { get; set; } // 근로자 ID
        public string 생산자Id { get; set; }
        public string 생산자인력매칭신청Id { get; set; }
        public string 주소 { get; set; }
        public DateTime 신청일자 { get; set; } // 근로 신청 날짜
        // 추가적인 속성 예시
        public string 작업종류 { get; set; } // 작업 종류
        public string 신청상태 { get; set; } // 신청 상태 (예: 대기 중, 승인됨, 완료됨)
    }
}
