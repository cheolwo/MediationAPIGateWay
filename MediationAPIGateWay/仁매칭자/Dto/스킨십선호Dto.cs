using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 매칭Infra;

namespace 仁매칭자.Dto
{
    // 사용자 선호를 입력받는 Dto
    public class 스킨십선호Dto
    {
        public string 사용자Id { get; set; } // 사용자 ID
        public 스킨십단계 최소허용스킨십 { get; set; } // 최소 허용 스킨십 단계
        public 스킨십단계 최대허용스킨십 { get; set; } // 최대 허용 스킨십 단계
        public int 배차Id { get; set; } // 배차 ID
        public string 좌석번호 { get; set; } // 좌석 번호
    }
}
