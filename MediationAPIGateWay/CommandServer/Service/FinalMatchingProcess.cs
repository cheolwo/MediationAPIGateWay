using 仁Common.근로자;

namespace CommandServer.Service
{
    public class FinalMatchingProcess
    {
        private readonly MatchingFilter _filter;
        private readonly MatchingService _matchingService;
        private readonly MatchingHistoryService _historyService;

        public FinalMatchingProcess(MatchingFilter filter, MatchingService matchingService, MatchingHistoryService historyService)
        {
            _filter = filter;
            _matchingService = matchingService;
            _historyService = historyService;
        }

        public Worker PerformFinalMatching(Worker user, List<Worker> candidates)
        {
            // 1. 필터링을 통한 후보자 선정
            var filteredCandidates = _filter.ApplyFilters(candidates, user);

            // 2. 점수 계산 및 랜덤 매칭
            var selectedMatch = _matchingService.PerformMatching(filteredCandidates, user);

            // 3. 매칭 이력 확인 및 재매칭 여부 확인
            if (_historyService.HasMatchingHistory(user, selectedMatch))
            {
                if (!_historyService.ConfirmReMatching(user, selectedMatch))
                {
                    // 재매칭 원하지 않으면 null 반환 또는 다른 후보 매칭
                    return null;
                }
            }
            return selectedMatch; // 최종 매칭된 사용자 반환
        }
    }
}
