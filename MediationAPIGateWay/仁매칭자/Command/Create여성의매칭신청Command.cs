using 仁매칭자.Dto;

namespace 仁매칭자.Command
{
    // 여성 매칭 신청 Command
    public class Create여성의매칭신청Command : Create매칭신청Command
    {

        public Create여성의매칭신청Command(매칭신청Dto dto)
            : base(dto)
        {
        }
    }
}
