using 셔틀Infra.Model;
using 셔틀Infra;
using Microsoft.EntityFrameworkCore;
using 仁근로자;

namespace MediationAPIGateWay.Service
{
    public class 셔틀버스서비스
    {
        private readonly 셔틀DbContext _context;

        public 셔틀버스서비스(셔틀DbContext context)
        {
            _context = context;
        }

        // 근로자 조회용: 이름을 마스킹 처리한 고객 정보
        public async Task<List<고객정보Dto>> Get고객정보For근로자(int 지하철역Id)
        {
            return await _context.셔틀버스배차들
                .Where(s => s.지하철역Id == 지하철역Id)
                .SelectMany(s => s.고객들)
                .Select(c => new 고객정보Dto
                {
                    이름 = c.이름.GetMaskedName(),// 이름 마스킹 처리
                    성별 = c.성별
                })
                .ToListAsync();
        }

        // 관리자 조회용: 모든 고객 정보 제공
        public async Task<List<고객정보Dto>> Get고객정보For관리자(int 지하철역Id)
        {
            return await _context.셔틀버스배차들
                .Where(s => s.지하철역Id == 지하철역Id)
                .SelectMany(s => s.고객들)
                .Select(c => new 고객정보Dto
                {
                    이름 = c.이름,
                    성별 = c.성별,
                    전화번호 = c.전화번호
                })
                .ToListAsync();
        }
        // 고객을 특정 셔틀버스 배차에 등록
        public async Task Register고객To셔틀버스(int 고객Id, int 지하철역Id)
        {
            // 셔틀버스배차 조회
            var 셔틀배차 = await _context.셔틀버스배차들
                .FirstOrDefaultAsync(s => s.지하철역Id == 지하철역Id);

            if (셔틀배차 == null)
            {
                throw new ApplicationException("해당 지하철역에 배차된 셔틀버스가 없습니다.");
            }

            // 고객 등록
            var 고객 = await _context.고객정보들.FirstOrDefaultAsync(c => c.Id == 고객Id);
            if (고객 != null)
            {
                셔틀배차.고객들.Add(고객);
                await _context.SaveChangesAsync();
            }
        }
    }
}
