using 생산Infra;

namespace 仁중재자.Service
{
    public class 보증금Service
    {
        private readonly 생산DbContext _context;

        public 보증금Service(생산DbContext context)
        {
            _context = context;
        }

        // 보증금 입금 여부 확인
        public async Task<bool> Check보증금(string 사용자Id, int 근무지Id)
        {
            // 보증금 테이블에서 해당 사용자와 근무지에 대한 보증금 입금 여부 확인
            var 보증금 = await _context.보증금들
                .FirstOrDefaultAsync(b => b.사용자Id == 사용자Id && b.근무지Id == 근무지Id);

            // 보증금이 입금된 경우 true 반환
            return 보증금 != null && 보증금.입금상태 == "입금완료";
        }
    }
}
