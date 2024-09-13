using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Infra
{
    // 서명 검증 요청 모델
    public class VerifyRequest
    {
        public string UserId { get; set; }
        public string Data { get; set; }
        public string Signature { get; set; }
    }

}
