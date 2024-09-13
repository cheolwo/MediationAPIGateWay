using 仁도서관련자.Query;

namespace MediationAPIGateWay.Service.도서
{
    public class 도서점수Service
    {
        // 키워드 목록을 기반으로 도서 목록을 점수화하여 정렬
        public List<도서Response> 정렬된도서목록(List<도서Response> 도서목록, List<string> 키워드목록)
        {
            // 우선순위 큐에 점수가 높은 도서를 먼저 삽입
            var 우선순위큐 = new PriorityQueue<도서Response, int>();

            foreach (var 도서 in 도서목록)
            {
                int 점수 = 계산된점수(도서, 키워드목록);

                // 점수가 0보다 크면 우선순위 큐에 삽입
                if (점수 > 0)
                {
                    우선순위큐.Enqueue(도서, -점수);  // 점수가 클수록 우선순위가 높음
                }
            }

            // 큐에서 하나씩 꺼내어 정렬된 목록 생성
            var 정렬된목록 = new List<도서Response>();
            while (우선순위큐.Count > 0)
            {
                정렬된목록.Add(우선순위큐.Dequeue());
            }

            return 정렬된목록;
        }

        // 도서 점수 계산 (키워드 일치도 및 적재일 기반)
        private int 계산된점수(도서Response 도서, List<string> 키워드목록)
        {
            int 점수 = 0;

            // 키워드 일치도에 따라 점수 부여
            foreach (var 키워드 in 키워드목록)
            {
                if (도서.제목.Contains(키워드, StringComparison.OrdinalIgnoreCase))
                {
                    점수 += 10; // 제목에 키워드가 포함된 경우 높은 점수
                }

                if (도서.내용.Contains(키워드, StringComparison.OrdinalIgnoreCase))
                {
                    점수 += 5; // 내용에 키워드가 포함된 경우
                }
            }

            // 적재일을 고려한 추가 점수 (오래된 도서일수록 높은 점수)
            int 도서적재일점수 = (DateTime.Now - 도서.적재일).Days / 10;
            점수 += 도서적재일점수;

            return 점수;
        }
    }
}
