using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace 이성Infra
{
    public class 이성DbContext : DbContext
    {
        public 이성DbContext(DbContextOptions<이성DbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<스킨십선호정보>()
                .HasOne(s => s.배차)
                .WithMany(b => b.스킨십선호정보들)
                .HasForeignKey(s => s.배차Id);

            modelBuilder.Entity<투표>(entity =>
            {
                entity.Property(e => e.투표안건Json).HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<투표안건>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                );
            });
        }
        public DbSet<투표> 투표들 { get; set; }
        public DbSet<스킨십선호정보> 스킨십선호정보들 { get; set; }
        public DbSet<배차> 배차들 { get; set; }
        public DbSet<이성매칭> 이성매칭목록 { get; set; }  // 매칭 테이블
    }
}
