using 仁Common.Command;

namespace 仁근로자.Command
{
    // 근로 후기 Command
    public class Create근로후기Command : Create후기Command
    {
        public int 근무지Id { get; set; }  // 근무지 ID

        public Create근로후기Command(string 작성자Id, string 내용, int 평점, int 근무지Id)
            : base(작성자Id, 내용, 평점)
        {
            this.근무지Id = 근무지Id;
        }
    }
}
