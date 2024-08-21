namespace 仁농협.Dto
{
    public class 근로신청승인요청Dto
    {
        public int 근로신청Id { get; set; }  // 근로신청 ID
        public string 농협담당자Id { get; set; } // 농협 담당자 ID
    }
    public class 근로신청복수승인요청Dto
    {
        public List<int> 근로신청Ids { get; set; }  // 승인할 근로신청들의 ID 리스트
        public string 농협담당자Id { get; set; }    // 농협 담당자 ID
    }
}
