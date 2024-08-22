using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 매칭Infra;

namespace 仁매칭자.Dto
{
    public class 매칭신청Dto
    {
        public string 사용자Id { get; set; }  // 사용자 ID
        public string 성별 { get; set; }        // 사용자 성별
        public int 나이 { get; set; }           // 사용자 나이
        public int 최소나이 { get; set; }  // 선호 최소 나이
        public int 최대나이 { get; set; }  // 선호 최대 나이
        public string 연상연하선호 { get; set; }  // 연상/연하 선호
        public string 기타선호사항 { get; set; }  // 기타 선호 사항
        public 스킨십단계 형식상최소허용스킨십단계 { get; set; } // 사용자가 선택한 최소 허용 스킨십 단계
        public 스킨십단계 형식상최대허용스킨십단계 { get; set; } // 사용자가 선택한 최대 허용 스킨십 단계
    }
}
