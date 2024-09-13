using MediatR;

namespace 仁도서관련자.Command
{
    public class Create도서Command : IRequest<bool>
    {
        public string 제목 { get; set; }
        public string 저자 { get; set; }
        public string 내용 { get; set; }
        public DateTime 적재일 { get; set; }

        public Create도서Command(string 제목, string 저자, string 내용, DateTime 적재일)
        {
            this.제목 = 제목;
            this.저자 = 저자;
            this.내용 = 내용;
            this.적재일 = 적재일;
        }
    }
}
