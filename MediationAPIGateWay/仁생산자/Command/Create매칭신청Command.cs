using MediatR;
using 仁Infra.생산자.DTO;
using 仁Common.Command;
using 사용자Infra;

namespace 仁생산자.Command
{
    public class Create인력매칭신청Command : IRequest<Unit>, IUserCommand
    {
        public int 사용자Id { get; set; }
        public int 근무지Id { get; set; }
        public int 생산자Id { get; set; }
        public string 이름 { get; set; } // 생산자 이름
        public string 연락처 { get; set; } // 생산자 연락처
        public string 주소 { get; set; } // 생산자 주소
        public string 설명 { get; set; } // 생산자 설명
        public DateTime 근무시작일 { get; set; }
        public DateTime 근무종료일 { get; set; }

        public 사용자 사용자 => throw new NotImplementedException();

        public string RequestUrl => throw new NotImplementedException();

        public string Context => "생산";

        public Create인력매칭신청Command(인력매칭신청DTO dto)
        {
            사용자Id = dto.사용자Id;
            근무지Id = dto.근무지Id;
            생산자Id = dto.생산자Id;
            근무시작일 = dto.근무시작일;
            근무종료일 = dto.근무종료일;
        }
    }
}
