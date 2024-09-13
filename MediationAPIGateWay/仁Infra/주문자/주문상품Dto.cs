using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Infra.주문자
{
    public class 주문상품Dto
    {
        public int 상품Id { get; set; }
        public string 상품명 { get; set; }
        public int 수량 { get; set; }
        public decimal 가격 { get; set; }
    }
}
