namespace 근로Infra
{
    public class 근로자결제
    {
        public int 결제Id { get; set; } // 기본 키
        public string 사용자Id { get; set; } // 사용자 ID
        public decimal 금액 { get; set; } // 결제 금액
        public DateTime 결제일자 { get; set; } // 결제 날짜
        public DateTime 만료일자 { get; set; } // 프리미엄 서비스 만료일
    }
    public class 근로신청
    {
        public int 근로신청Id { get; set; } // 기본 키
        public string 근로자Id { get; set; } // 근로자 ID
        public string 성별 { get; set; } // 성별
        public int 나이 { get; set; } // 나이
        public bool 매칭원함 { get; set; } // 매칭을 원하는지 여부
        public int 나이범위최소 { get; set; } // 선호하는 나이 범위 (최소)
        public int 나이범위최대 { get; set; } // 선호하는 나이 범위 (최대)
        public bool 연상여성선호 { get; set; } = true; // 여성이 연상인 매칭을 원하는지 여부
        public bool 결제기능해금 { get; set; } // 결제를 통한 프리미엄 기능 해금 여부
        public DateTime 신청일자 { get; set; } // 근로 신청 날짜
    }
}
