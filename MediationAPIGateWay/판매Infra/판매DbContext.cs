using Microsoft.EntityFrameworkCore;
using 판매Infra.Model;

namespace 판매Infra
{
    // 판매DbContext
    public class 판매DbContext : DbContext
    {
        public 판매DbContext(DbContextOptions<판매DbContext> options) : base(options) { }

        // DbSet 정의: 각 엔티티에 대응하는 테이블
        public DbSet<판매자> 판매자들 { get; set; }  // 판매자 테이블
        public DbSet<판매상품> 판매상품들 { get; set; }  // 판매상품 테이블
        public DbSet<상품상세정보> 상품상세정보들 { get; set; }  // 상품상세정보 테이블
        public DbSet<후기> 후기들 { get; set; }  // 후기 테이블

        // 테이블 간 관계 설정
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 판매자 - 판매상품 1:N 관계
            modelBuilder.Entity<판매자>()
                .HasMany(p => p.판매상품목록)
                .WithOne(s => s.판매자)
                .HasForeignKey(s => s.판매자Id);

            // 판매상품 - 상품상세정보 1:1 관계
            modelBuilder.Entity<판매상품>()
                .HasOne(s => s.상세정보)
                .WithOne()
                .HasForeignKey<판매상품>(s => s.상품상세정보Id);

            // 상품상세정보 - 후기 1:N 관계
            modelBuilder.Entity<상품상세정보>()
                .HasMany(s => s.후기목록)
                .WithOne(f => f.상품상세정보)
                .HasForeignKey(f => f.상품상세정보Id);
        }
    }
}
