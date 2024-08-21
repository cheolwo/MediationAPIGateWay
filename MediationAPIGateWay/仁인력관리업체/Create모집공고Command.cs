using MediatR;

namespace 인력관리업체.Command
{
    // 근로자 모집 공고 생성
    public class Create모집공고Command : IRequest
    {
        public string 모집코드 { get; set; }
        public string 작업명 { get; set; }
        public int 필요인원수 { get; set; }
        public DateTime 시작일자 { get; set; }
        public DateTime 종료일자 { get; set; }
    }

    // 근로자 배치 업데이트
    public class Update배치Command : IRequest
    {
        public string 배치코드 { get; set; }
        public string 근로자ID { get; set; }
        public string 작업명 { get; set; }
    }

    // 근로자 모집 공고 삭제
    public class Delete모집공고Command : IRequest
    {
        public string 모집코드 { get; set; }
    }
}
