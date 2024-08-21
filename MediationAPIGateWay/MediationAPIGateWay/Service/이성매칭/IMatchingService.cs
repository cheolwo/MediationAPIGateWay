using Microsoft.EntityFrameworkCore;
using 매칭Infra;

namespace MediationAPIGateWay.Service.이성매칭
{
    public interface IMatchingService
    {
        Task<매칭?> MatchAsync(매칭 신청, CancellationToken cancellationToken);
    }
    public class MatchingService : IMatchingService
    {
        private readonly 매칭DbContext _context;

        public MatchingService(매칭DbContext context)
        {
            _context = context;
        }

        public async Task<매칭?> MatchAsync(매칭 신청, CancellationToken cancellationToken)
        {
            // 간단한 매칭 로직
            var 매칭대상자 = await _context.매칭들
                .Where(m => m.선호정보.성별 == 신청.선호정보.성별 &&
                            m.선호정보.최소나이 <= 신청.선호정보.최대나이 &&
                            m.선호정보.최대나이 >= 신청.선호정보.최소나이)
                .FirstOrDefaultAsync(cancellationToken);

            return 매칭대상자;
        }
    }
}
