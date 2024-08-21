using MediatR;
using 仁근로자;

namespace 仁Common.근로자
{
    // 근로신청 Command
    public class Create근로신청Command : IRequest<Unit>
    {
        private 결제요청Dto 결제요청Dto1;

        public string 사용자Id { get; set; }
        public int 근무지Id { get; set; }
        public DateTime 근무시작일 { get; set; }
        public DateTime 근무종료일 { get; set; }
        public 결제요청Dto 결제요청Dto { get; set; }
        public bool 이성매칭여부 { get; set; }
        public bool 자차여부 { get; set; }
        public 셔틀버스배차요청Dto 셔틀버스배차요청Dto { get; set; } // 셔틀버스 정보 추가

        public Create근로신청Command(
            string 사용자Id,
            int 근무지Id,
            DateTime 근무시작일,
            DateTime 근무종료일,
            결제요청Dto 결제요청Dto,
            bool 이성매칭여부,
            bool 자차여부,
            셔틀버스배차요청Dto 셔틀버스배차요청Dto) // 셔틀버스 정보도 생성자에 추가
        {
            this.사용자Id = 사용자Id;
            this.근무지Id = 근무지Id;
            this.근무시작일 = 근무시작일;
            this.근무종료일 = 근무종료일;
            this.결제요청Dto = 결제요청Dto;
            this.이성매칭여부 = 이성매칭여부;
            this.자차여부 = 자차여부;
            this.셔틀버스배차요청Dto = 셔틀버스배차요청Dto; // 셔틀버스 정보 저장
        }

        public Create근로신청Command(string 사용자Id, int 근무지Id, DateTime 근무시작일, DateTime 근무종료일, 결제요청Dto 결제요청Dto1, bool 이성매칭여부, bool 자차여부)
        {
            this.사용자Id = 사용자Id;
            this.근무지Id = 근무지Id;
            this.근무시작일 = 근무시작일;
            this.근무종료일 = 근무종료일;
            this.결제요청Dto1 = 결제요청Dto1;
            this.이성매칭여부 = 이성매칭여부;
            this.자차여부 = 자차여부;
        }
    }
}
