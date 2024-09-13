namespace 仁Infra
{
    public class 결제요청Dto
    {
        public string 사용자Id { get; set; }
        public string 결제고유번호 { get; set; }  // 페이사에서 제공하는 결제 고유번호
        public string 거래번호 { get; set; }     // 각 거래에 대한 고유 거래번호
        public string 결제방법 { get; set; }      // 결제 방법 (카카오페이, 네이버페이, 토스페이 등)
        public decimal 결제금액 { get; set; }     // 결제 금액
        public DateTime 결제일시 { get; set; }    // 결제가 이루어진 일시
    }
}
