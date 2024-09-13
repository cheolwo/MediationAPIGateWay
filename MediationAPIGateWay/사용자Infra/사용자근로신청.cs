namespace 사용자Infra
{
    public class 사용자근로신청
    {
        public int 근로신청Id { get; set; } // 기본 키
        public string 근로자Id { get; set; } // 근로자 ID
        public string 성별 { get; set; } // 성별
        public int 나이 { get; set; } // 나이
        public DateTime 신청일자 { get; set; } // 근로 신청 날짜
    }
}
