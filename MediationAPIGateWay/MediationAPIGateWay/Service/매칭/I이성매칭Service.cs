using Microsoft.EntityFrameworkCore;
using 매칭Infra;

namespace MediationAPIGateWay.Service.매칭
{
    public interface I이성매칭Service
    {
        Task<이성매칭?> MatchAsync(이성매칭 신청, CancellationToken cancellationToken);
    }
    public class 이성매칭Service : I이성매칭Service
    {
        private readonly 이성DbContext _context;

        public 이성매칭Service(이성DbContext context)
        {
            _context = context;
        }

        public async Task<이성매칭?> MatchAsync(이성매칭 신청, CancellationToken cancellationToken)
        {
            // 간단한 매칭 로직
            var 매칭대상자 = await _context.이성매칭목록
                .Where(m => m.선호정보.성별 == 신청.선호정보.성별 &&
                            m.선호정보.최소나이 <= 신청.선호정보.최대나이 &&
                            m.선호정보.최대나이 >= 신청.선호정보.최소나이)
                .FirstOrDefaultAsync(cancellationToken);

            return 매칭대상자;
        }
    }
}
