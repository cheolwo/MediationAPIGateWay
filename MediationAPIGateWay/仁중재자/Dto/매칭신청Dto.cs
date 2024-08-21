using MediatR;

namespace 仁중재자.Dto
{
    public class 매칭신청Dto
    {
        public string 사용자Id { get; set; }
        public int 근무지Id { get; set; }
    }

    public class ProcessMatchingCommand : IRequest
    {
        public string 사용자Id { get; set; }
        public int 근무지Id { get; set; }

        public ProcessMatchingCommand(string 사용자Id, int 근무지Id)
        {
            this.사용자Id = 사용자Id;
            this.근무지Id = 근무지Id;
        }
    }
}
