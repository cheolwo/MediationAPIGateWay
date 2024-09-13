using Microsoft.EntityFrameworkCore;

namespace 도서Infra
{
    public class 도서DbContext : DbContext
    {
        public 도서DbContext(DbContextOptions<도서DbContext> options) : base(options) { }

        // DbSet 정의: 각 엔티티에 대응하는 테이블
        public DbSet<도서> 도서들 { get; set; } // 도서 테이블
        public DbSet<도서적재> 도서적재들 { get; set; } // 도서적재 테이블
        public DbSet<선반> 선반들 { get; set; } // 선반 테이블
        public DbSet<대출반납기록> 대출반납기록들 { get; set; } // 대출/반납 기록 테이블

        // 테이블 간 관계 설정
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 도서 - 도서적재 1:1 관계
            modelBuilder.Entity<도서>()
                .HasOne(d => d.도서적재)
                .WithOne(dr => dr.도서)
                .HasForeignKey<도서적재>(dr => dr.도서Id);

            // 도서적재 - 선반 1:N 관계
            modelBuilder.Entity<도서적재>()
                .HasMany(dr => dr.선반목록)
                .WithOne(s => s.도서적재)
                .HasForeignKey(s => s.도서적재Id);

            // 도서 - 대출반납기록 1:N 관계
            modelBuilder.Entity<도서>()
                .HasMany(d => d.대출반납기록들)
                .WithOne(r => r.도서)
                .HasForeignKey(r => r.도서Id);
        }
    }

    public class 도서
    {
        public int Id { get; set; } // 도서 ID
        public string 제목 { get; set; } // 도서 제목
        public string 저자 { get; set; } // 도서 저자
        public string 내용 { get; set; } // 도서의 내용
        public 도서적재 도서적재 { get; set; } // 도서적재 참조
        public List<대출반납기록> 대출반납기록들 { get; set; } // 대출/반납 기록 참조
    }

    public class 도서적재
    {
        public int Id { get; set; } // 도서적재 ID
        public int 도서Id { get; set; } // 도서 ID (Foreign Key)
        public DateTime 적재일 { get; set; } // 적재일
        public List<선반> 선반목록 { get; set; } // 선반 목록
        public 도서 도서 { get; set; } // 도서 참조
    }
    public class 선반
    {
        public int Id { get; set; } // 선반 ID
        public int 도서적재Id { get; set; } // 도서적재 ID (Foreign Key)
        public string 위치 { get; set; } // 선반 위치
        public 도서적재 도서적재 { get; set; } // 도서적재 참조
    }

    public class 대출반납기록
    {
        public int Id { get; set; } // 대출/반납 기록 ID
        public int 도서Id { get; set; } // 도서 ID (Foreign Key)
        public DateTime 대출일 { get; set; } // 대출일
        public DateTime? 반납일 { get; set; } // 반납일 (nullable)
        public string 대출자 { get; set; } // 대출자 이름
        public 도서 도서 { get; set; } // 도서 참조
    }
}
