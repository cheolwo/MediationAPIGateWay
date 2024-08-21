using Microsoft.EntityFrameworkCore;

namespace 근로Infra
{
    public class 근로자결제Repository
    {
        private readonly 근로DbContext _context;

        public 근로자결제Repository(근로DbContext context)
        {
            _context = context;
        }

        public async Task 결제추가Async(근로자결제 결제)
        {
            _context.결제정보.Add(결제);
            await _context.SaveChangesAsync();
        }

        public async Task<근로자결제> 사용자최근결제정보(string 사용자Id)
        {
            return await _context.결제정보
                .Where(p => p.사용자Id == 사용자Id)
                .OrderByDescending(p => p.결제일자)
                .FirstOrDefaultAsync();
        }
    }
    public class 근로신청Repository
    {
        private readonly 근로DbContext _context;

        public 근로신청Repository(근로DbContext context)
        {
            _context = context;
        }

        public async Task 근로신청추가Async(근로신청 근로신청)
        {
            _context.근로신청정보.Add(근로신청);
            await _context.SaveChangesAsync();
        }

        public async Task<근로신청> 근로자신청정보(string 근로자Id)
        {
            return await _context.근로신청정보
                .FirstOrDefaultAsync(j => j.근로자Id == 근로자Id);
        }
    }
}
