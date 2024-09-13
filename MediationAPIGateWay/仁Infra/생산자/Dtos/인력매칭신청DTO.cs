namespace 仁Infra.생산자.DTO
{
    public class 인력매칭신청DTO
    {
        public int 인력매칭신청Id { get; set; }
        public int 사용자Id { get; set; }
        public int 근무지Id { get; set; }
        public int 생산자Id { get; set; }
        public int 근로자Id { get; set; }
        public DateTime 근무시작일 { get; set; }
        public DateTime 근무종료일 { get; set; }
        public string 매칭상태 { get; set; }
    }
}
