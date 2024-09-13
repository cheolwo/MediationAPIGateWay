using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace 사용자Infra
{
    /// <summary>
    /// 주체지향성, 통합성, 비휘발성
    /// </summary>
    public class 사용자
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string 이름 { get; set; }
        public string 성별 { get; set; }
        public string 전화번호 { get; set; }
        public X509 공개키인증서 { get; set; }
        public string FcmToken { get; set; }
        public DateTime 생성일 { get; set; }
        public DateTime 만료일 { get; set; }

        // RDBMS 객체를 JSON으로 직렬화하여 MongoDB에 저장
        public string 주문자Json { get; set; }
        public string 생산자Json { get; set; }
        public string 근로자Json { get; set; }
        public string 배송자Json { get; set; }
    }
    public class X509
    {
        public string 인증서_일련번호 { get; set; }
        public string 사용자Id { get; set; }
        public string 공개키 { get; set; }
        public DateTime 생성일 { get; set; }
        public DateTime 만료일 { get; set; }
        public string SHA_Value { get; set; }
        public string 서버_전자서명 { get; set; }
    }
}
