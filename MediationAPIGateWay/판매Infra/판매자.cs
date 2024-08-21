
namespace 판매Infra.Model
{
    // 판매자 엔티티
    public class 판매자
    {
        public int Id { get; set; }  // 판매자 ID
        public string 이름 { get; set; }  // 판매자 이름
        public string 연락처 { get; set; }  // 판매자 연락처
        public string 주소 { get; set; }  // 판매자 주소
        public List<판매상품> 판매상품목록 { get; set; }  // 판매자가 관리하는 상품 목록
    }

    // 판매상품 엔티티
    public class 판매상품
    {
        public int Id { get; set; }  // 판매상품 ID
        public string 이름 { get; set; }  // 상품 이름
        public string 상품코드 { get; set; }  // 상품 코드
        public decimal 가격 { get; set; }  // 상품 가격
        public int 생산자Id { get; set; }
        public int 농협Id { get; set; }
        public int 판매자Id { get; set; }  // 외래키 (판매자 ID)
        public 판매자 판매자 { get; set; }  // 판매자와의 관계

        public int 상품상세정보Id { get; set; }  // 외래키 (상품상세정보 ID)
        public 상품상세정보 상세정보 { get; set; }  // 1:1 관계로 상품 상세정보
    }

    // 상품상세정보 엔티티
    public class 상품상세정보
    {
        public int Id { get; set; }  // 상품상세정보 ID
        public string 설명 { get; set; }  // 상품 설명
        public string 원산지 { get; set; }  // 원산지 정보
        public bool 관계매칭지원가능 { get; set; }  // 관계매칭 지원 여부
        public bool 근로매칭지원가능 { get; set; }  // 근로매칭 지원 여부
        public List<후기> 후기목록 { get; set; }  // 상품에 대한 후기 목록
    }

    // 후기 엔티티
    public class 후기
    {
        public int Id { get; set; }  // 후기 ID
        public string 작성자Id { get; set; }  // 작성자 ID
        public string 내용 { get; set; }  // 후기 내용
        public int 평점 { get; set; }  // 평점
        public DateTime 작성일 { get; set; }  // 작성일
        public bool 비공개여부 { get; set; }  // 비공개 여부
        public int 상품상세정보Id { get; set; }  // 외래키 (상품상세정보 ID)
        public 상품상세정보 상품상세정보 { get; set; }  // 상품상세정보와의 관계
    }
}
