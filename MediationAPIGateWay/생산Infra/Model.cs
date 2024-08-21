namespace 생산Infra.Model
{
    // 농협 엔티티
    public class 농협
    {
        public int Id { get; set; }
        public string 이름 { get; set; } // 농협 이름
        public string 주소 { get; set; } // 농협 주소
        public List<생산자> 생산자목록 { get; set; } // 농협이 관리하는 생산자 목록
        public List<생산상품> 생산상품목록 { get; set; } // 농협이 관리하는 생산상품 목록
        public List<후기> 후기목록 { get; set; } // 생산자에 대한 후기 목록
    }

    // 생산상품 엔티티
    public class 생산상품
    {
        public int Id { get; set; }
        public string 이름 { get; set; } // 상품 이름
        public string 상품코드 { get; set; } // 상품 코드
        public string 설명 { get; set; } // 상품 설명
        public decimal 가격 { get; set; } // 상품 가격

        // 외래키 설정
        public int 농협Id { get; set; } // 농협 ID (외래키)
        public 농협 농협 { get; set; } // 농협과의 관계

        public int 생산자Id { get; set; } // 생산자 ID (외래키)
        public 생산자 생산자 { get; set; } // 생산자와의 관계
    }

    // 생산자 엔티티
    public class 생산자
    {
        public int Id { get; set; }
        public string 이름 { get; set; } // 생산자 이름
        public string 연락처 { get; set; } // 생산자 연락처
        public string 주소 { get; set; } // 생산자 주소
        public string 설명 { get; set; } // 생산자 설명

        // 외래키 설정
        public int 농협Id { get; set; } // 농협 ID (외래키)
        public 농협 농협 { get; set; } // 농협과의 관계

        public List<생산상품> 생산상품목록 { get; set; } // 생산자가 관리하는 상품 목록
        public List<근무지> 근무지목록 { get; set; } // 생산자가 관리하는 근무지 목록
        public List<후기> 후기목록 { get; set; } // 생산자에 대한 후기 목록
    }

    // 근무지 엔티티
    public class 근무지
    {
        public int Id { get; set; }
        public string 이름 { get; set; } // 근무지 이름
        public string 위치 { get; set; } // 근무지 위치
        public string 설명 { get; set; } // 근무지 설명

        // 외래키 설정
        public int 생산자Id { get; set; } // 생산자 ID (외래키)
        public 생산자 생산자 { get; set; } // 생산자와의 관계
        // 외래키 설정
        public int 농협Id { get; set; } // 농협 ID (외래키)
        public 농협 농협{ get; set; } // 농협과의 관계
    }

    // 후기 엔티티
    public class 후기
    {
        public int Id { get; set; }
        public string 작성자Id { get; set; } // 작성자 ID
        public string 내용 { get; set; } // 후기 내용
        public int 평점 { get; set; } // 평점 (1~5)
        public DateTime 작성일 { get; set; } // 작성일

        // 외래키 설정
        public int 생산자Id { get; set; } // 생산자 ID
        public 생산자 생산자 { get; set; } // 생산자와의 관계
        // 외래키 설정
        public int 농협Id { get; set; } // 농협 ID (외래키)
        public 농협 농협 { get; set; } // 농협과의 관계
    }

    // 근로신청 엔티티
    public class 근로신청
    {
        public int Id { get; set; }
        public string 사용자Id { get; set; } // 신청한 사용자 ID

        // 외래키 설정
        public int 근무지Id { get; set; } // 신청한 근무지 ID
        public 근무지 근무지 { get; set; } // 근무지와의 관계

        public DateTime 근무시작일 { get; set; } // 근무 시작일
        public DateTime 근무종료일 { get; set; } // 근무 종료일

        // 보증금 관련 속성
        public bool 보증금납부여부 { get; set; } // 보증금 납부 여부
        public decimal 설정보증금액 { get; set; } // 설정된 보증금액

        // 승인 관련 속성
        public bool 승인상태 { get; set; }  // 승인 여부
        public string 승인자Id { get; set; }  // 승인한 농협 담당자 ID
        public DateTime? 승인일자 { get; set; }  // 승인일자
    }
}
