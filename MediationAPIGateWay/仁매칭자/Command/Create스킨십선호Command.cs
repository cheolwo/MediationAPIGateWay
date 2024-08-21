using MediatR;
using 매칭Infra;
using 仁매칭자.Dto;

namespace 仁매칭자.Command
{
    public class Create스킨십선호Command : IRequest<Unit>
    {
        public string 사용자Id { get; set; }
        public 스킨십단계 최소허용스킨십 { get; set; }
        public 스킨십단계 최대허용스킨십 { get; set; }
        public int 배차Id { get; set; } // 배차 ID 추가
        public string 좌석번호 { get; set; } // 좌석 번호 추가

        public Create스킨십선호Command(스킨십선호Dto dto)
        {
            사용자Id = dto.사용자Id;
            최소허용스킨십 = dto.최소허용스킨십;
            최대허용스킨십 = dto.최대허용스킨십;
            배차Id = dto.배차Id;
            좌석번호 = dto.좌석번호;
        }
    }
}
