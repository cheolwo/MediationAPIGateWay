using MediatR;
using 仁매칭자.Dto;

namespace 仁매칭자.Command
{
    public class Create매칭신청Command : IRequest<Unit>
    {
        public string 사용자Id { get; set; }   // 사용자 ID
        public string 성별 { get; set; }        // 사용자 성별
        public int 나이 { get; set; }           // 사용자 나이
        public int 최소나이 { get; set; }       // 선호하는 최소 나이
        public int 최대나이 { get; set; }       // 선호하는 최대 나이
        public string 연상연하선호 { get; set; } // 연상/연하 선호
        public string 기타선호사항 { get; set; } // 기타 선호사항

        public Create매칭신청Command(매칭신청Dto dto)
        {
            사용자Id = dto.사용자Id;
            성별 = dto.성별;
            나이 = dto.나이;
            최소나이 = dto.최소나이;
            최대나이 = dto.최대나이;
            연상연하선호 = dto.연상연하선호;
            기타선호사항 = dto.기타선호사항;
        }
    }

}
