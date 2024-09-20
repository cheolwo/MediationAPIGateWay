using Microsoft.EntityFrameworkCore;
using 셔틀Infra.Model;

namespace 셔틀Infra
{
    public class 마차DbContext : DbContext
    {
        public DbSet<고객정보> 고객정보목록 { get; set; }
        public DbSet<셔틀마차배차> 셔틀마차배차목록 { get; set; }
        public DbSet<마을> 마을목록 { get; set; }
        public DbSet<셔틀마차> 셔틀마차목록 { get; set; }
        public DbSet<수정구> 수정구목록 { get; set; }

        public 마차DbContext(DbContextOptions<마차DbContext> options) : base(options) { }
    }

}
