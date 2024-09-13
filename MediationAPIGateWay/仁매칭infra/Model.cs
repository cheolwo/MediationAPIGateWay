using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace 仁매칭infra
{
    public class 근로자정보
    {
        public string 근로자Id { get; set; }
        public string 이름 { get; set; }
        public string 연락처 { get; set; }
        public string 상태 { get; set; } // 예: 대기중, 선택됨, 거절됨 등
    }

    public class 근로인력매칭
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int 생산자Id { get; set; }
        public string 이름 { get; set; } // 생산자 이름
        public string 연락처 { get; set; } // 생산자 연락처
        public string 주소 { get; set; } // 생산자 주소
        public string 설명 { get; set; } // 생산자 설명
        public int 근무지Id { get; set; }
        public List<근로자정보> 근로자목록 { get; set; }
        public DateTime 매칭신청일자 { get; set; }
        public int 선택된근로자Id { get; set; } // 생산자가 선택한 근로자 ID
    }

    public class 이성매칭신청
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string 사용자Id { get; set; }
        public string 상태 { get; set; } // 신청 상태 (대기, 승인 등)
    }
    public class 셔틀매칭신청
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string 셔틀버스Id { get; set; }
        public string 상태 { get; set; } // 신청 상태
    }
    public class 주문매칭신청
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int 주문자집단Id { get; set; }
        public string 주소 { get; set; } //위도, 경도
        public string 매칭상태 { get; set; } // 매칭 상태
    }
    public class 배송매칭신청
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int 배송자Id { get; set; }
        public string 주소 { get; set; } //위도, 경도
        public string 매칭상태 { get; set; }
    }
    public class 판로매칭신청
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int 판매자Id { get; set; }
        public int 생산자Id { get; set; }
        public string 주소 { get; set; } //위도, 경도
        public string 매칭상태 { get; set; }
    }
    public class 물류매칭계약
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int 주문자집단Id { get; set; }  // 주문자 집단 ID
        
        // 한 사용자가 여러 역할을 가질 수 있는 형태
        public Dictionary<int, List<UserRole>> 참여자목록 { get; set; }  // 참여자 목록 (UserId, Role)

        // 계약 서명 정보
        public Dictionary<string, string> 계약서명목록 { get; set; }

        // 주문 상품별 물류 이동 단계 목록
        public Dictionary<int, List<LogisticsStep>> 이동순서 { get; set; }  // Key는 주문상품Id
    }
    public class LogisticsStep
    {
        public string 위치 { get; set; }  // 물류가 이동할 위치
        public UserRole 담당자역할 { get; set; }  // 담당자의 역할 (배송자, 생산자 등)
        public int 담당자Id { get; set; }  // 담당자의 ID
        public DateTime 예정시간 { get; set; }  // 물류 이동 예정 시간
        public DateTime? 완료시간 { get; set; }  // 물류 이동 완료 시간
        public bool 완료여부 { get; set; }  // 이동 완료 여부
    }
    public enum UserRole
    {
        배송자,  // Delivery Person
        생산자,  // Producer
        관리자,  // Admin (추가적인 역할 가능)
        고객      // Customer
    }
}
