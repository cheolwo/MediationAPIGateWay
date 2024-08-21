using MediatR;

namespace 仁농협.Command
{
    public class Approve근로신청Command : IRequest<Unit>
    {
        public int 근로신청Id { get; set; }
        public string 농협담당자Id { get; set; }

        public Approve근로신청Command(int 근로신청Id, string 농협담당자Id)
        {
            this.근로신청Id = 근로신청Id;
            this.농협담당자Id = 농협담당자Id;
        }
    }
    public class Approve근로신청들Command : IRequest<Unit>
    {
        public List<int> 근로신청Ids { get; set; }
        public string 농협담당자Id { get; set; }

        public Approve근로신청들Command(List<int> 근로신청Ids, string 농협담당자Id)
        {
            this.근로신청Ids = 근로신청Ids;
            this.농협담당자Id = 농협담당자Id;
        }
    }
}
