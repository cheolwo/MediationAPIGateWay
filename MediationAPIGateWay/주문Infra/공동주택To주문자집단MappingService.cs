using AutoMapper;
using 국토교통부_공공데이터Common.Model;
using Microsoft.EntityFrameworkCore;
using 국토교통부_공공데이터Common;

namespace 주문Infra
{
    public class 공동주택To주문자집단MappingService
    {
        private readonly 주문DbContext _주문Context;
        private readonly 공동주택DbContext _공동주택Context;
        private readonly IMapper _mapper;

        public 공동주택To주문자집단MappingService(주문DbContext 주문Context, 공동주택DbContext 공동주택Context, IMapper mapper)
        {
            _주문Context = 주문Context;
            _공동주택Context = 공동주택Context;
            _mapper = mapper;
        }

        public async Task MapAndSaveAsync(공동주택 공동주택)
        {
            var 주문자집단 = _mapper.Map<주문자집단>(공동주택);
            _주문Context.주문자집단목록.Add(주문자집단);
            await _주문Context.SaveChangesAsync();
        }

        public async Task SyncAll공동주택To주문자집단Async()
        {
            var 공동주택목록 = await _공동주택Context.공동주택목록.ToListAsync();

            foreach (var 공동주택 in 공동주택목록)
            {
                // 동일한 단지코드를 가진 주문자집단이 있는지 확인
                var existing주문자집단 = await _주문Context.주문자집단목록
                    .FirstOrDefaultAsync(j => j.단지코드 == 공동주택.단지코드);

                if (existing주문자집단 == null)
                {
                    // 매핑하고 추가
                    var 주문자집단 = _mapper.Map<주문자집단>(공동주택);
                    _주문Context.주문자집단목록.Add(주문자집단);
                }
            }

            // 변경 사항 저장
            await _주문Context.SaveChangesAsync();
        }
    }
}
