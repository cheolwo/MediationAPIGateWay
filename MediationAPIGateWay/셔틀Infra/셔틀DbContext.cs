using Microsoft.EntityFrameworkCore;
using 셔틀Infra.Model;

namespace 셔틀Infra
{
    public class 셔틀DbContext : DbContext
    {
        public 셔틀DbContext(DbContextOptions<셔틀DbContext> options) : base(options) { }

        // DbSet 정의: 각 엔티티를 관리하는 테이블
        public DbSet<고객정보> 고객정보들 { get; set; }  // 고객 정보 테이블
        public DbSet<셔틀버스배차> 셔틀버스배차들 { get; set; }  // 셔틀버스 배차 테이블
        public DbSet<지하철역> 지하철역들 { get; set; }  // 지하철역 테이블
        public DbSet<셔틀버스> 셔틀버스들 { get; set; }  // 셔틀버스 테이블
        public DbSet<고속도로휴게소> 고속도로휴게소들 { get; set; }  // 고속도로 휴게소 테이블
        public DbSet<셔틀버스휴게소> 셔틀버스휴게소들 { get; set; }  // 조인 테이블

        // 테이블 간 관계 설정
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 셔틀버스 배차 테이블 설정
            modelBuilder.Entity<셔틀버스배차>()
                .HasKey(s => s.Id);  // 기본키 설정

            // 고객 정보와 셔틀버스 배차 간의 관계 설정 (1:N)
            modelBuilder.Entity<셔틀버스배차>()
                .HasMany(s => s.고객들)  // 셔틀버스 배차가 여러 고객 정보를 가질 수 있음
                .WithOne()  // 고객 정보는 하나의 셔틀버스 배차에 속함
                .HasForeignKey("셔틀버스배차Id");  // 고객 정보의 외래키로 셔틀버스배차Id 설정

            // 셔틀버스와 고속도로 휴게소 간의 다대다 관계 설정 (조인 테이블 사용)
            modelBuilder.Entity<셔틀버스휴게소>()
                .HasKey(bh => new { bh.셔틀버스Id, bh.휴게소Id });  // 복합키 설정

            modelBuilder.Entity<셔틀버스휴게소>()
                .HasOne(bh => bh.셔틀버스)
                .WithMany(b => b.셔틀버스휴게소들)
                .HasForeignKey(bh => bh.셔틀버스Id);

            modelBuilder.Entity<셔틀버스휴게소>()
                .HasOne(bh => bh.고속도로휴게소)
                .WithMany(h => h.셔틀버스휴게소들)
                .HasForeignKey(bh => bh.휴게소Id);

            // 지하철역 테이블 설정
            modelBuilder.Entity<지하철역>()
                .HasKey(z => z.Id);  // 기본키 설정

            // 셔틀버스 테이블 설정
            modelBuilder.Entity<셔틀버스>()
                .HasKey(b => b.Id);  // 기본키 설정

            // 고속도로 휴게소 테이블 설정
            modelBuilder.Entity<고속도로휴게소>()
                .HasKey(h => h.Id);  // 기본키 설정
        }
    }
}
