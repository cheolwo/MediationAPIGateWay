using Common.Model;

namespace 이성Infra
{
    public enum 스킨십단계
    {
        노터치 = 1,
        손잡기,
        어깨터치,
        허벅지터치,
        가슴터치,
        생식기접촉
    }
    public class 투표 : Entity
    {
        public int 셔틀버스배차Id { get; set; }
        public string 투표주제 { get; set; }
        public List<투표안건> 투표안건Json { get; set; }  // JSON 형태로 저장되는 투표안건
        public DateTime 투표시작시간 { get; set; }
        public DateTime 투표종료시간 { get; set; }
    }
    public class 투표안건 : Entity
    {
        public string 내용 { get; set; }
        public int 찬성투표수 { get; set; }
        public int 반대투표수 { get; set; }
    }

    public class 이성매칭 : Entity
    {
        public string 사용자Id { get; set; }  // 사용자 ID (외래키로 매칭되는 사용자)
        public int 근무지Id { get; set; }  // 근무지 ID (외래키로 매칭되는 근무지)
        public DateTime 신청일자 { get; set; }  // 매칭 신청 일자
        public bool 매칭완료여부 { get; set; }  // 매칭 완료 여부 (true: 완료, false: 미완료)
        public string 가능매칭구간 { get; set; }  // 매칭 가능한 구간 (예: 지역, 시간대 등)
        public string 성별 { get; set; }  // 선호 성별
        public int 최소나이 { get; set; }  // 선호 최소 나이
        public int 최대나이 { get; set; }  // 선호 최대 나이
        public string 기타선호사항 { get; set; }  // 기타 선호 사항 (선호하는 성격, 조건 등)
        public 스킨십단계 형식상최소허용스킨십단계 { get; set; } // 사용자가 선택한 최소 허용 스킨십 단계
        public 스킨십단계 형식상최대허용스킨십단계 { get; set; } // 사용자가 선택한 최대 허용 스킨십 단계
    }
    public class 스킨십선호정보
    {
        public int Id { get; set; } // Primary Key
        public string 사용자Id { get; set; } // 사용자 ID
        public 스킨십단계 최소허용스킨십 { get; set; } // 최소 허용 스킨십
        public 스킨십단계 최대허용스킨십 { get; set; } // 최대 허용 스킨십

        // 배차 및 좌석 정보 추가
        public int 배차Id { get; set; } // 배차 ID (외래키)
        public string 좌석번호 { get; set; } // 좌석 번호
        public 배차 배차 { get; set; } // 배차와의 관계 설정 (Navigation Property)
    }
    public class 배차
    {
        public int Id { get; set; }  // 배차 ID (Primary Key)
        public DateTime 배차일자 { get; set; }  // 배차 일자
        public string 버스번호 { get; set; }  // 버스 번호
        public List<스킨십선호정보> 스킨십선호정보들 { get; set; } // 스킨십 선호 정보 리스트 (Navigation Property)
    }
}
