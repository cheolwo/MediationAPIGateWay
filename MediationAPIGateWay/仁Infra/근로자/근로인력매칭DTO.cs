using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Infra.근로자
{
    public class 근로인력매칭DTO
    {
        public string 매칭신청Id { get; set; }
        public string 생산자Id { get; set; }
        public string 생산자이름 { get; set; }
        public DateTime 매칭신청일자 { get; set; }
        public List<근로자정보DTO> 근로자목록 { get; set; } // DTO를 사용하여 근로자 정보 제공
    }

    public class 근로자정보DTO
    {
        public string 근로자Id { get; set; }
        public string 이름 { get; set; }
        public string 연락처 { get; set; }
        public string 상태 { get; set; } // 근로자의 매칭 상태
    }

}
