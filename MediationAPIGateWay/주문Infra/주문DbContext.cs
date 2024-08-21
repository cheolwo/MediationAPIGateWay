using Microsoft.EntityFrameworkCore;
using 주문Common.Model;

namespace 주문Infra
{
    public class 주문DbContext : DbContext
    {
        public 주문DbContext(DbContextOptions<주문DbContext> options) : base(options) { }

        public DbSet<주문자집단> 주문자집단들 { get; set; }
        public DbSet<공동주문상품> 공동주문상품들 { get; set; }
        public DbSet<개별주문상품> 개별주문상품들 { get; set; }  // 개별주문상품 테이블 추가
        public DbSet<주문자> 주문자들 { get; set; }
        public DbSet<주문상품> 주문상품들 { get; set; }
        public DbSet<생산자> 생산자들 { get; set; }
        public DbSet<주문상태> 주문상태들 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 주문자집단과 공동주문상품 간의 관계 설정
            modelBuilder.Entity<주문자집단>()
                .HasMany(j => j.공동주문상품들)
                .WithOne(p => p.주문자집단)
                .HasForeignKey(p => p.집단코드);

            // 주문자와 주문상품 간의 관계 설정
            modelBuilder.Entity<주문자>()
                .HasMany(j => j.주문상품들)
                .WithOne(p => p.주문자)
                .HasForeignKey(p => p.주문자Id);

            // 생산자와 공동주문상품 간의 관계 설정
            modelBuilder.Entity<생산자>()
                .HasMany(s => s.공동주문상품들)
                .WithOne(p => p.생산자)
                .HasForeignKey(p => p.생산자Id);

            // 생산자와 개별주문상품 간의 관계 설정
            modelBuilder.Entity<생산자>()
                .HasMany(s => s.개별주문상품들)
                .WithOne(p => p.생산자)
                .HasForeignKey(p => p.생산자Id);
        }
    }
}

