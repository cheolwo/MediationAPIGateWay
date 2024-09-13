using Common.Model;

namespace 생산Infra.Model
{
    public enum 매칭상태
    {
        대기중,
        매칭완료,
        거절됨
    }

    // 농협 엔티티
    public class 농협 : Entity
    {
        public string 이름 { get; set; } // 농협 이름
        public string 주소 { get; set; } // 농협 주소
        public List<생산자> 생산자목록 { get; set; } // 농협이 관리하는 생산자 목록
        public List<생산상품> 생산상품목록 { get; set; } // 농협이 관리하는 생산상품 목록
        public List<후기> 후기목록 { get; set; } // 생산자에 대한 후기 목록
    }

    // 생산상품 엔티티
    public class 생산상품 : Entity
    {
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
    public class 생산자 : Entity
    {
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
        public List<생산자인력매칭신청> 생산자인력매칭신청목록 { get; set; }
    }

    // 근무지 엔티티
    public class 근무지 : Entity
    {
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
    public class 후기 : Entity
    {
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

    public class 생산자인력매칭신청 : Entity
    {
        public int 사용자Id { get; set; }
        public int 근무지Id { get; set; }
        public 근무지 근무지 { get; set; }
        public int 생산자Id { get; set; }
        public 생산자 생산자 { get; set; }
        public DateTime 근무시작일 { get; set; }
        public DateTime 근무종료일 { get; set; }
        public bool 승인상태 { get; set; }
        public string 승인자Id { get; set; }
        public DateTime? 승인일자 { get; set; }

        // 추가된 매칭 상태
        public 매칭상태 매칭상태 { get; set; } = 매칭상태.대기중;
    }

}
