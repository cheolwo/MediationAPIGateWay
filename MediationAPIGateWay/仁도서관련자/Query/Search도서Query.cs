using MediatR;

namespace 仁도서관련자.Query
{
    public class Search도서Query : IRequest<List<도서Response>>
    {
        public List<string> 키워드목록 { get; set; }  // 다중 키워드 목록

        public Search도서Query(List<string> 키워드목록)
        {
            this.키워드목록 = 키워드목록;
        }
    }

    public class 도서Response
    {
        public int Id { get; set; }
        public string 제목 { get; set; } // 도서 제목
        public string 저자 { get; set; } // 도서 저자
        public string 내용 { get; set; } // 도서 내용
        public DateTime 적재일 { get; set; } // 도서 적재일
        public int 점수 { get; set; } // 도서의 점수 (키워드와의 일치도에 기반한 점수)
    }
}
