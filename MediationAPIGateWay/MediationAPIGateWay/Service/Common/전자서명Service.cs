using System.Security.Cryptography;
using System.Text;
using 사용자Infra;
using 仁주문자.For주문자.Command;

namespace MediationAPIGateWay.Service
{
    public class 전자서명Service
    {
        public bool VerifySignature(Create공동주문Command command, X509 인증서)
        {
            using (RSA rsa = RSA.Create())
            {
                // 공개키를 이용하여 RSA 객체 초기화
                rsa.FromXmlString(인증서.공개키);

                // 원본 데이터 생성
                string 원본데이터 = command.집단코드 + string.Join(",", command.공동주문상품들.Select(x => x.상품Id));
                byte[] 원본데이터Bytes = Encoding.UTF8.GetBytes(원본데이터);

                // 서명 데이터
                byte[] 서명Bytes = Convert.FromBase64String(command.전자서명);

                // 서명 검증
                return rsa.VerifyData(원본데이터Bytes, 서명Bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }

}
