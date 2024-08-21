using MediatR;

namespace 仁Common.Command
{
    // 공통 후기 Command
    public abstract class Create후기Command : IRequest<Unit>
    {
        public string 작성자Id { get; set; }  // 작성자 ID
        public string 내용 { get; set; }  // 후기 내용
        public int 평점 { get; set; }  // 평점 (1~5)
        public DateTime 작성일 { get; set; }  // 작성일자
        public bool 비공개여부 { get; set; }  // 비공개 여부

        // 생성자에서 비공개 여부를 설정하는 공통 로직
        public Create후기Command(string 작성자Id, string 내용, int 평점)
        {
            this.작성자Id = 작성자Id;
            this.내용 = 내용;
            this.평점 = 평점;
            this.작성일 = DateTime.UtcNow;

            // 별점이 3.5 이하일 경우 비공개 처리
            this.비공개여부 = 평점 <= 3.5;
        }
    }
}
