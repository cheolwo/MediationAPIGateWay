namespace 사용자Infra
{
    public class 사용자결제
    {
        public int Id { get; set; }
        public decimal 금액 { get; set; }
        public DateTime 결제일자 { get; set; }
        public DateTime 만료일자 { get; set; }
    }
}
