using Microsoft.EntityFrameworkCore;
using 주문Infra.Model;

namespace 주문Infra
{
    public class 주문DbContext : DbContext
    {
        public 주문DbContext(DbContextOptions<주문DbContext> options) : base(options) { }

        public DbSet<주문자집단> 주문자집단목록 { get; set; }
        public DbSet<주문상품> 주문상품목록 { get; set; }  // 개별주문상품 테이블
        public DbSet<주문자> 주문자목록 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 주문자집단 - 주문자 관계 설정 (1:N)
            modelBuilder.Entity<주문자집단>()
                .HasMany(jg => jg.주문자들)
                .WithMany(j => j.주문자집단목록)
                .UsingEntity<Dictionary<string, object>>(
                    "주문자집단주문자",
                    j => j.HasOne<주문자>().WithMany().HasForeignKey("주문자Id"),
                    jg => jg.HasOne<주문자집단>().WithMany().HasForeignKey("주문자집단Id")
                );

            // 주문자 - 주문상품 관계 설정 (1:N)
            modelBuilder.Entity<주문자>()
                .HasMany(j => j.주문상품목록)
                .WithOne(o => o.주문자)
                .HasForeignKey(o => o.주문자Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 주문자집단 - 주문상품 관계 설정 (1:N)
            modelBuilder.Entity<주문자집단>()
                .HasMany(jg => jg.주문상품목록)
                .WithOne(o => o.주문자집단)
                .HasForeignKey(o => o.주문자집단Id)
                .OnDelete(DeleteBehavior.SetNull); // 주문자집단 삭제 시 주문상품에서 주문자집단Id를 null로 설정

            // 기타 속성 및 제약 조건 설정
            modelBuilder.Entity<주문자>()
                .Property(j => j.이름)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<주문상품>()
                .Property(o => o.가격)
                .HasColumnType("decimal(18,2)");

            // 기본 값 설정
            modelBuilder.Entity<주문상품>()
                .Property(o => o.할인금액)
                .HasDefaultValue(0m);

            modelBuilder.Entity<주문상품>()
                .Property(o => o.최종결제금액)
                .HasComputedColumnSql("[가격] - [할인금액]"); // SQL로 계산된 열로 설정

            base.OnModelCreating(modelBuilder);
        }
    }
}

