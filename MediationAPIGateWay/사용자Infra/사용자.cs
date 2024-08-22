using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace 사용자Infra
{
    public class 사용자
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string 이름 { get; set; }
        public List<사용자후기> 후기들 { get; set; }
        public List<사용자근로신청> 근로신청들 { get; set; }
        public List<사용자매칭신청> 매칭신청들 { get; set; }
        public List<사용자고객정보> 고객정보들 { get; set; }
        public List<사용자결제> 사용자결제들 { get; set; }
    }

    public class 사용자후기
    {
        public int Id { get; set; }
        public string 내용 { get; set; }
        public int 평점 { get; set; }
        public DateTime 작성일 { get; set; }
    }
    public class 사용자매칭신청
    {
        public int Id { get; set; }  // 매칭 ID (Primary Key)
        public string 사용자Id { get; set; }  // 사용자 ID (외래키로 매칭되는 사용자)
        public int 근무지Id { get; set; }  // 근무지 ID (외래키로 매칭되는 근무지)
        public DateTime 신청일자 { get; set; }  // 매칭 신청 일자
        public bool 매칭완료여부 { get; set; }  // 매칭 완료 여부 (true: 완료, false: 미완료)
        public string 가능매칭구간 { get; set; }  // 매칭 가능한 구간 (예: 지역, 시간대 등)
        public string 성별 { get; set; }  // 선호 성별
        public int 최소나이 { get; set; }  // 선호 최소 나이
        public int 최대나이 { get; set; }  // 선호 최대 나이
        public string 기타선호사항 { get; set; }  // 기타 선호 사항 (선호하는 성격, 조건 등)
        public string 형식상최소허용스킨십단계 { get; set; } // 사용자가 선택한 최소 허용 스킨십 단계
        public string 형식상최대허용스킨십단계 { get; set; } // 사용자가 선택한 최대 허용 스킨십 단계
    }

    public class 사용자근로신청
    {
        public int 근로신청Id { get; set; } // 기본 키
        public string 근로자Id { get; set; } // 근로자 ID
        public string 성별 { get; set; } // 성별
        public int 나이 { get; set; } // 나이
        public DateTime 신청일자 { get; set; } // 근로 신청 날짜
    }

    public class 사용자고객정보
    {
        public int Id { get; set; }
        public string 이름 { get; set; }
        public string 성별 { get; set; }
        public string 전화번호 { get; set; }
    }

    public class 사용자결제
    {
        public int Id { get; set; }
        public decimal 금액 { get; set; }
        public DateTime 결제일자 { get; set; }
        public DateTime 만료일자 { get; set; }
    }
}
