using Microsoft.EntityFrameworkCore;

namespace 매칭Infra
{
    public enum 스킨십단계
    {
        손잡기 = 1,
        어깨터치,
        허벅지터치,
        가슴터치,
        생식기접촉
    }
    public class 매칭
    {
        public int Id { get; set; }  // 매칭 ID (Primary Key)
        public string 사용자Id { get; set; }  // 사용자 ID
        public int 근무지Id { get; set; }  // 근무지 ID
        public DateTime 신청일자 { get; set; }  // 매칭 신청 일자
        public bool 매칭완료여부 { get; set; }  // 매칭 완료 여부
        public string 가능매칭구간 { get;  set; }
        public 매칭선호정보? 선호정보 { get; set; }
    }
    public class 매칭선호정보
    {
        public int Id { get; set; }
        public string 사용자Id { get; set; } // 사용자 ID
        public int 최소나이 { get; set; } // 선호 최소 나이
        public int 최대나이 { get; set; } // 선호 최대 나이
        public string 연상연하선호 { get; set; } // 연상/연하 선호
        public string 기타선호사항 { get; set; } // 기타 선호 사항
    }
    public class 매칭DbContext : DbContext
    {
        public 매칭DbContext(DbContextOptions<매칭DbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<스킨십선호정보>()
                .HasOne(s => s.배차)
                .WithMany(b => b.스킨십선호정보들)
                .HasForeignKey(s => s.배차Id);
        }
        public DbSet<스킨십선호정보> 스킨십선호정보들 { get; set; }
        public DbSet<배차> 배차들 { get; set; }
        public DbSet<매칭> 매칭들 { get; set; }  // 매칭 테이블
        public DbSet<매칭선호정보> 매칭선호정보들 { get; set; }  // 매칭선호정보 테이블
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
