using Microsoft.EntityFrameworkCore;
using 생산Infra.Model;

namespace 생산Infra
{
    public class 생산DbContext : DbContext
    {
        public 생산DbContext(DbContextOptions<생산DbContext> options) : base(options) { }

        // DbSet 정의: 각 엔티티를 관리하는 테이블
        public DbSet<농협> 농협들 { get; set; }  // 농협 테이블
        public DbSet<생산자> 생산자들 { get; set; }  // 생산자 테이블
        public DbSet<근무지> 근무지들 { get; set; }  // 근무지 테이블
        public DbSet<후기> 후기들 { get; set; }  // 후기 테이블
        public DbSet<근로신청> 근로신청들 { get; set; }  // 근로신청 테이블

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 농협과 생산자 간의 1:N 관계 설정
            modelBuilder.Entity<농협>()
                .HasMany(n => n.생산자목록)
                .WithOne(p => p.농협)
                .HasForeignKey(p => p.농협Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 농협과 생산상품 간의 1:N 관계 설정
            modelBuilder.Entity<농협>()
                .HasMany(n => n.생산상품목록)
                .WithOne(p => p.농협)
                .HasForeignKey(p => p.농협Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 생산자와 근무지 간의 1:N 관계 설정
            modelBuilder.Entity<생산자>()
                .HasMany(p => p.근무지목록)
                .WithOne(g => g.생산자)
                .HasForeignKey(g => g.생산자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 생산자와 후기 간의 1:N 관계 설정
            modelBuilder.Entity<생산자>()
                .HasMany(p => p.후기목록)
                .WithOne(f => f.생산자)
                .HasForeignKey(f => f.생산자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 생산자와 생산상품 간의 1:N 관계 설정
            modelBuilder.Entity<생산자>()
                .HasMany(p => p.생산상품목록)
                .WithOne(p => p.생산자)
                .HasForeignKey(p => p.생산자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 근무지와 근로신청 간의 1:N 관계 설정
            modelBuilder.Entity<근무지>()
                .HasMany(w => w.근로신청들)
                .WithOne(ws => ws.근무지)
                .HasForeignKey(ws => ws.근무지Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 기타 설정
            base.OnModelCreating(modelBuilder);
        }
    }
}
