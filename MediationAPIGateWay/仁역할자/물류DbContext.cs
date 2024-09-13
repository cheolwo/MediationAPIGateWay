using Microsoft.EntityFrameworkCore;

namespace 仁물류Infra
{
    public class 물류DbContext : DbContext
    {
        public 물류DbContext(DbContextOptions<물류DbContext> options) : base(options) { }

        // DbSet 정의: 각 엔티티를 관리하는 테이블
        public DbSet<물류관리자> 물류관리자목록 { get; set; }
        public DbSet<물류상품> 물류상품목록 { get; set; }
        public DbSet<입고기록> 입고기록목록 { get; set; }
        public DbSet<적재기록> 적재기록목록 { get; set; }
        public DbSet<출고기록> 출고기록목록 { get; set; }
        public DbSet<물류비용예측> 물류비용예측목록 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 물류관리자와 입고기록 간의 1:N 관계 설정
            modelBuilder.Entity<물류관리자>()
                .HasMany(m => m.입고기록목록)
                .WithOne(i => i.물류관리자)
                .HasForeignKey(i => i.물류관리자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 물류관리자와 적재기록 간의 1:N 관계 설정
            modelBuilder.Entity<물류관리자>()
                .HasMany(m => m.적재기록목록)
                .WithOne(j => j.물류관리자)
                .HasForeignKey(j => j.물류관리자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 물류관리자와 출고기록 간의 1:N 관계 설정
            modelBuilder.Entity<물류관리자>()
                .HasMany(m => m.출고기록목록)
                .WithOne(o => o.물류관리자)
                .HasForeignKey(o => o.물류관리자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 물류상품과 입고기록 간의 1:1 관계 설정
            modelBuilder.Entity<입고기록>()
                .HasOne(i => i.물류상품)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            // 물류상품과 적재기록 간의 1:1 관계 설정
            modelBuilder.Entity<적재기록>()
                .HasOne(j => j.물류상품)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            // 물류상품과 출고기록 간의 1:1 관계 설정
            modelBuilder.Entity<출고기록>()
                .HasOne(o => o.물류상품)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            // 기타 설정
            base.OnModelCreating(modelBuilder);
        }
    }
}
