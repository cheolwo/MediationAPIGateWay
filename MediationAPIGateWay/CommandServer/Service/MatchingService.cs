using 仁Common.근로자;

namespace CommandServer.Service
{
    public class MatchingFilter
    {
        public List<Worker> ApplyFilters(List<Worker> candidates, Worker user)
        {
            return candidates
                .Where(candidate =>
                    candidate.Gender != user.Gender &&  // 성별이 다를 경우
                    candidate.Age >= user.AgeRangeMin && candidate.Age <= user.AgeRangeMax &&  // 나이 범위 조건
                    Math.Abs(candidate.MuscleMass - user.MuscleMass) <= 5 &&  // 근육량 조건
                    Math.Abs(candidate.BodyFatPercentage - user.BodyFatPercentage) <= 5)  // 체지방량 조건
                .ToList();
        }
    }
    public class MatchingScoreCalculator
    {
        public int CalculateScore(Worker user, Worker candidate)
        {
            int score = 0;

            // 나이 점수
            if (candidate.Age >= user.AgeRangeMin && candidate.Age <= user.AgeRangeMax)
            {
                score += 10;
            }

            // 근육량 점수
            score += CalculateBodyScore(user, candidate);

            // 성격, 취향 점수 추가 가능
            return score;
        }

        private int CalculateBodyScore(Worker user, Worker candidate)
        {
            int bodyScore = 0;
            if (Math.Abs(user.MuscleMass - candidate.MuscleMass) <= 5)
            {
                bodyScore += 5;
            }
            if (Math.Abs(user.BodyFatPercentage - candidate.BodyFatPercentage) <= 5)
            {
                bodyScore += 5;
            }
            return bodyScore;
        }
    }
    public class MatchingService
    {
        public Worker PerformMatching(List<Worker> candidates, Worker user)
        {
            // 필터링된 후보자들에게 점수 부여
            var scoredCandidates = candidates
                .Select(candidate => new { Candidate = candidate, Score = new MatchingScoreCalculator().CalculateScore(user, candidate) })
                .OrderByDescending(result => result.Score)
                .ToList();

            // 최고 점수를 받은 후보자들 추출
            int highestScore = scoredCandidates.First().Score;
            var topCandidates = scoredCandidates.Where(result => result.Score == highestScore).Select(result => result.Candidate).ToList();

            // 최종 후보가 2명 이상일 경우 랜덤으로 선택
            if (topCandidates.Count > 1)
            {
                Random random = new Random();
                return topCandidates[random.Next(topCandidates.Count)];
            }

            // 유일한 최고 점수 후보자 반환
            return topCandidates.First();
        }
    }
    public class MatchingHistoryService
    {
        // 매칭 이력 확인
        public bool HasMatchingHistory(Worker user, Worker candidate)
        {
            // 실제 매칭 이력을 조회 (DB 등에서 확인)
            return false; // 기본적으로 이력이 없는 경우 false 반환
        }

        // 재매칭 여부를 사용자에게 확인
        public bool ConfirmReMatching(Worker user, Worker candidate)
        {
            Console.WriteLine($"{user.Name}, {candidate.Name}와 다시 매칭하시겠습니까?");
            // UI나 API로 사용자의 선택을 받음
            return true; // 기본적으로 재매칭 원할 경우 true 반환
        }
    }
}
