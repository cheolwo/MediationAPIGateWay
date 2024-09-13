using Common.Model;
using Microsoft.EntityFrameworkCore;

namespace 결제Infra
{
    public class 결제DbContext : DbContext
    {
        public 결제DbContext(DbContextOptions<결제DbContext> options) : base(options) { }

        public DbSet<결제내역> 결제내역들 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<결제내역>().HasKey(p => p.Id);
        }
    }

    public class 결제내역 : Entity
    {
        public string 사용자Id { get; set; }
        public string 결제방법 { get; set; }  // 카카오, 네이버, 토스 등
        public decimal 결제금액 { get; set; }
        public string 거래번호 { get; set; }
        public DateTime 결제일시 { get; set; }
        public string 결제상태 { get; set; }  // 승인, 취소 등
    }
}
