namespace 셔틀Infra.Model
{
    // 고객 정보를 저장하는 엔티티
    public class 고객정보
    {
        public int Id { get; set; }  // 고객 ID (Primary Key)
        public string 이름 { get; set; }  // 고객 이름
        public string 성별 { get; set; }  // 고객 성별
        public string 전화번호 { get; set; }  // 고객 전화번호

        // 외래키 - 셔틀버스배차
        public int 셔틀버스배차Id { get; set; }  // 외래키
        public 셔틀버스배차 셔틀버스배차 { get; set; }  // Navigation Property (1:N 관계)
    }

    // 셔틀버스 배치 정보를 저장하는 엔티티
    public class 셔틀버스배차
    {
        public int Id { get; set; }  // 배차 ID (Primary Key)
        public int 지하철역Id { get; set; }  // 선택한 지하철역 ID (외래키)
        public int 셔틀버스Id { get; set; }  // 셔틀버스 ID (외래키)
        public DateTime 출발시간 { get; set; }  // 셔틀버스 출발 시간
        public DateTime 배차일자 { get; set; }  // 셔틀버스 배치 일자

        // 고객 리스트 - 1:N 관계
        public List<고객정보> 고객들 { get; set; }  // 고객 정보 (Navigation Property)

        // Many-to-Many 관계: 셔틀버스와 휴게소 간의 경유 관계
        public List<셔틀버스휴게소> 셔틀버스휴게소들 { get; set; }  // 셔틀버스 - 휴게소 경유 정보
    }

    // 지하철역 정보를 저장하는 엔티티
    public class 지하철역
    {
        public int Id { get; set; }  // 지하철역 ID (Primary Key)
        public string 호선명 { get; set; }  // 지하철 호선명
        public string 역명 { get; set; }  // 지하철역 이름
    }

    // 셔틀버스 정보를 저장하는 엔티티
    public class 셔틀버스
    {
        public int Id { get; set; }  // 셔틀버스 ID (Primary Key)
        public string 회사 { get; set; }  // 셔틀버스 회사명
        public string 버스번호 { get; set; }  // 셔틀버스 번호
        public string 버스기사명 { get; set; }  // 버스 기사 이름

        // Many-to-Many 관계: 셔틀버스와 휴게소 간의 경유 관계
        public List<셔틀버스휴게소> 셔틀버스휴게소들 { get; set; }  // 셔틀버스 - 휴게소 경유 정보
    }

    // 고속도로 휴게소 정보를 저장하는 엔티티
    public class 고속도로휴게소
    {
        public int Id { get; set; }  // 휴게소 ID (Primary Key)
        public string 휴게소명 { get; set; }  // 휴게소 이름
        public string 위치 { get; set; }  // 휴게소 위치

        // Many-to-Many 관계: 셔틀버스와 휴게소 간의 경유 관계
        public List<셔틀버스휴게소> 셔틀버스휴게소들 { get; set; }  // 셔틀버스 - 휴게소 경유 정보
    }

    // 셔틀버스와 고속도로 휴게소 간의 Many-to-Many 관계를 정의하는 조인 테이블
    public class 셔틀버스휴게소
    {
        public int 셔틀버스Id { get; set; }  // 셔틀버스 ID (외래키)
        public 셔틀버스 셔틀버스 { get; set; }  // 셔틀버스 Navigation Property

        public int 휴게소Id { get; set; }  // 고속도로 휴게소 ID (외래키)
        public 고속도로휴게소 고속도로휴게소 { get; set; }  // 고속도로 휴게소 Navigation Property
    }
}
