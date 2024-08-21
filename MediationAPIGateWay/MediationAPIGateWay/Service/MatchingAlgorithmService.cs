using Microsoft.EntityFrameworkCore;
using 매칭Infra;
using 생산Infra;

namespace MediationAPIGateWay.Service
{
    public class MatchingAlgorithmService
    {
        private readonly 매칭DbContext _매칭Context;
        private readonly 생산DbContext _생산Context;

        public MatchingAlgorithmService(매칭DbContext 매칭Context, 생산DbContext 생산Context)
        {
            _매칭Context = 매칭Context;
            _생산Context = 생산Context;
        }

        public async Task<List<매칭>> ProcessMatching()
        {
            // 1. 매칭 가능한 여성 목록을 불러오기 (여성만 우선 조회)
            var 매칭대기여성 = await _매칭Context.매칭들
                .Where(m => !m.매칭완료여부 && m.매칭선호정보 != null && m.매칭선호정보.성별 == "여성")
                .Include(m => m.매칭선호정보)
                .ToListAsync();

            var 매칭결과 = new List<매칭>();

            foreach (var 여성 in 매칭대기여성)
            {
                var 여성나이 = _생산Context.근로자들.FirstOrDefault(u => u.Id == 여성.사용자Id)?.나이 ?? 0;

                // 2.1 여성의 연하남 선호에 따라 후보군 필터링 (여성 나이보다 1~12살 연하 남성 필터링)
                var 가능한연하남 = await _매칭Context.매칭들
                    .Where(m => !m.매칭완료여부 && m.매칭선호정보 != null && m.매칭선호정보.성별 == "남성")
                    .Select(m => _생산Context.근로자들.FirstOrDefault(u => u.Id == m.사용자Id))
                    .Where(u => u != null && (여성나이 - u.나이) >= 1 && (여성나이 - u.나이) <= 12) // 연하 남성 필터링
                    .ToListAsync();

                if (가능한연하남.Any())
                {
                    // 2.2 점수 기반으로 남성 후보를 평가 (예: 나이 차이, 관심사 등으로 점수 매김)
                    var 최적의남성 = 가능한연하남.OrderByDescending(u => 점수계산(여성, u)).FirstOrDefault();

                    if (최적의남성 != null)
                    {
                        // 나이 차이에 따라 비용 분담 구조 설정
                        int 나이차이 = 여성나이 - 최적의남성.나이;

                        if (나이차이 >= 1 && 나이차이 <= 3)
                        {
                            // 기본 매칭, 비용 없음
                            Console.WriteLine("기본 매칭, 추가 비용 없음");
                        }
                        else if (나이차이 >= 4 && 나이차이 <= 6)
                        {
                            // 여성이 50% 부담, 남성 50% 부담
                            Console.WriteLine("매칭 비용을 여성 50%, 남성 50% 부담");
                        }
                        else if (나이차이 >= 7 && 나이차이 <= 9)
                        {
                            // 대형버스 임대비용을 여성이 부담
                            Console.WriteLine("대형버스 임대비용을 여성이 부담");
                        }
                        else if (나이차이 >= 10 && 나이차이 <= 12)
                        {
                            // 대형버스 임대비용과 숙박비를 여성이 부담
                            Console.WriteLine("대형버스 임대비용 및 숙박비를 여성이 부담");
                        }

                        // 매칭 완료 처리
                        여성.매칭완료여부 = true;
                        var 상대방매칭기록 = await _매칭Context.매칭들.FirstOrDefaultAsync(m => m.사용자Id == 최적의남성.Id);
                        if (상대방매칭기록 != null)
                        {
                            상대방매칭기록.매칭완료여부 = true;
                            매칭결과.Add(여성);
                            매칭결과.Add(상대방매칭기록);
                        }
                    }
                }
            }

            // 매칭 결과 저장
            await _매칭Context.SaveChangesAsync();

            return 매칭결과;
        }

        private int 점수계산(매칭 여성, 근로자 남성)
        {
            // 나이 차이에 따른 점수 계산 (나이 차이에 따라 점수 매김)
            int 점수 = 0;
            int 나이차이 = 여성.매칭선호정보.최소나이 - 남성.나이;

            if (나이차이 >= 1 && 나이차이 <= 3)
            {
                점수 += 10; // 연하남일수록 높은 점수
            }

            // 기타 선호 조건에 따라 점수 추가 (예: 관심사 일치, 성격 등)
            // ...

            return 점수;
        }
    }

}
