using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using 사용자Infra;
using MongoDB.Driver;

namespace MediationAPIGateWay.Service
{
    public class PKI_X509Service
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<RevokedCertificate> _revokedCertsCollection;

        public PKI_X509Service(IConfiguration configuration, IMongoClient mongoClient)
        {
            _configuration = configuration;
            var database = mongoClient.GetDatabase("PKIService");
            _revokedCertsCollection = database.GetCollection<RevokedCertificate>("RevokedCertificates");
        }

        // X.509 인증서 생성 메서드
        public X509 CreateCertificate(string userId, string userName)
        {
            using (RSA rsa = RSA.Create(GetKeySize()))
            {
                var certificateRequest = new CertificateRequest(
                    new X500DistinguishedName($"CN={userName}"), rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                var cert = certificateRequest.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1));

                // SHA 값과 서버 전자서명 생성
                var shaValue = Convert.ToBase64String(cert.GetCertHash());
                var signature = CreateServerSignature(cert.RawData);

                // X509 인증서 객체 생성
                var x509Cert = new X509
                {
                    인증서_일련번호 = cert.SerialNumber,
                    사용자Id = userId,
                    공개키 = Convert.ToBase64String(cert.GetPublicKey()),
                    생성일 = DateTime.UtcNow,
                    만료일 = DateTime.UtcNow.AddYears(1),
                    SHA_Value = shaValue,
                    서버_전자서명 = signature
                };

                return x509Cert;
            }
        }

        // 서버 전자서명 생성 (비밀키로 서명)
        public string CreateServerSignature(byte[] data)
        {
            var privateKeyString = GetServerPrivateKey();
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(privateKeyString.ToCharArray());
                var signedData = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signedData);
            }
        }

        // 인증서 서명 검증
        public bool VerifySignature(byte[] data, byte[] signature, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKey.ToCharArray());
                return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        // 폐지된 인증서인지 확인
        public async Task<bool> IsCertificateRevoked(string serialNumber)
        {
            return await _revokedCertsCollection.Find(c => c.SerialNumber == serialNumber).AnyAsync();
        }

        // 인증서 갱신
        public X509 RenewCertificate(X509 currentCert)
        {
            currentCert.생성일 = DateTime.UtcNow;
            currentCert.만료일 = DateTime.UtcNow.AddYears(1);
            return currentCert;
        }

        // 서버 비밀키 가져오기
        private string GetServerPrivateKey()
        {
            return _configuration["PKI:ServerPrivateKey"]; // 환경변수 또는 Key Vault 사용 가능
        }

        // RSA 키 크기 가져오기
        private int GetKeySize()
        {
            return _configuration.GetValue<int>("PKI:KeySize", 2048); // 기본값 2048
        }
        // 폐지된 인증서를 저장하고 조회할 수 있는 헬퍼 메서드
        public IMongoCollection<RevokedCertificate> GetRevokedCertificatesCollection()
        {
            return _revokedCertsCollection;
        }
    }

    // 폐지된 인증서 저장 모델
    public class RevokedCertificate
    {
        public string SerialNumber { get; set; }
        public DateTime RevokedDate { get; set; }
    }
}
