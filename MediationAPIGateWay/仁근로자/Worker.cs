using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common.근로자
{
    public class Worker
    {
        // 기본 정보
        public string WorkerId { get; set; }      // 근로자 ID (고유 식별자)
        public string Name { get; set; }          // 이름
        public string Gender { get; set; }        // 성별
        public int Age { get; set; }              // 나이

        // 신체 정보
        public float MuscleMass { get; set; }     // 근육량
        public float BodyFatPercentage { get; set; }  // 체지방량

        // 근로 신청 정보
        public string PreferredWorkLocation { get; set; }  // 선호하는 작업 장소
        public DateTime WorkStartDate { get; set; }        // 근무 시작일
        public DateTime WorkEndDate { get; set; }          // 근무 종료일

        // 매칭 관련 정보
        public bool IsMatchingDesired { get; set; }        // 짝을 원하는지 여부
        public int AgeRangeMin { get; set; }               // 선호하는 나이 범위 (최소)
        public int AgeRangeMax { get; set; }               // 선호하는 나이 범위 (최대)
        public bool IsExchangeRequested { get; set; }      // 교환 요청 여부

        // 추가 메타 정보 (선택 사항)
        public string Notes { get; set; }                  // 기타 메모
    }
}
