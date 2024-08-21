using 仁Common;

namespace 仁근로자
{
    public class 근로신청Dto
    {
        public string 사용자Id { get; set; } // 신청하는 사용자 ID
        public int 근무지Id { get; set; } // 신청할 근무지 ID
        public DateTime 근무시작일 { get; set; } // 근무 시작일
        public DateTime 근무종료일 { get; set; } // 근무 종료일
        public 결제요청Dto 결제요청Dto { get; set; } // 결제 요청 정보
        public bool 이성매칭여부 { get; set; } // 이성매칭 여부
        public bool 자차여부 { get; set; } // 자차 이용 여부
        public 셔틀버스배차요청Dto? 셔틀버스배차요청Dto { get; set; } // 셔틀버스 배차 요청 정보 (선택적)
    }
    public class 셔틀버스배차요청Dto
    {
        public int 사용자Id { get; set; }  // 근로자 ID
        public int 지하철역Id { get; set; }  // 선택한 지하철역 ID
        public int 셔틀버스Id { get; set; }  // 배차된 셔틀버스 ID
    }
    public class 고객정보Dto
    {
        public string 이름 { get; set; }  // 고객 이름
        public string 성별 { get; set; }  // 고객 성별
                                        // 근로자 조회 시 전화번호를 제외하고 조회
        public string 전화번호 { get; set; }  // 관리자 전용 조회용
    }
    public class 이성매칭신청Dto
    {
        public string 사용자Id { get; set; } // 신청하는 사용자 ID
        public string 성별 { get; set; } // 성별
        public int 최소나이 { get; set; } // 선호하는 최소 나이
        public int 최대나이 { get; set; } // 선호하는 최대 나이
        public string 연상연하선호 { get; set; } // 연상/연하 선호
        public string 기타선호사항 { get; set; } // 기타 선호 사항
    }
}
