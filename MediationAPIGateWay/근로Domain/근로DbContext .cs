using Microsoft.EntityFrameworkCore;

namespace 근로Infra
{
    public class 근로DbContext : DbContext
    {
        public 근로DbContext(DbContextOptions<근로DbContext> options) : base(options)
        {
        }

        public DbSet<근로자결제> 결제정보 { get; set; } // 결제 테이블
        public DbSet<근로신청> 근로신청정보 { get; set; } // 근로 신청 테이블
        public DbSet<근로자> 사용자정보 { get; set; } // 사용자 테이블

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 결제 테이블 구성
            modelBuilder.Entity<근로자결제>(entity =>
            {
                entity.HasKey(p => p.결제Id);
                entity.Property(p => p.근로자Id).IsRequired();
                entity.Property(p => p.금액).IsRequired();
                entity.Property(p => p.결제일자).IsRequired();
                entity.Property(p => p.만료일자).IsRequired();

                // 사용자와 외래키 관계 설정
                entity.HasOne<근로자>()
                      .WithMany(u => u.결제정보)
                      .HasForeignKey(p => p.근로자Id);
            });

            // 근로신청 테이블 구성
            modelBuilder.Entity<근로신청>(entity =>
            {
                entity.HasKey(j => j.근로신청Id);
                entity.Property(j => j.근로자Id).IsRequired();
                entity.Property(j => j.신청일자).IsRequired();

                // 사용자와 외래키 관계 설정
                entity.HasOne<근로자>()
                      .WithMany(u => u.근로신청들)
                      .HasForeignKey(j => j.근로자Id);
            });
        }
    }
}
