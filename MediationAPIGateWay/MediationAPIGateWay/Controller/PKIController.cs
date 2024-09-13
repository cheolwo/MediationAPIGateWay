using MediationAPIGateWay.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using 사용자Infra;
using 仁Infra;

namespace MediationAPIGateWay.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PkiController : ControllerBase
    {
        private readonly IMongoCollection<사용자> _userCollection;
        private readonly PKI_X509Service _pkiService;

        public PkiController(IMongoClient mongoClient, PKI_X509Service pkiService)
        {
            var database = mongoClient.GetDatabase("UserDb");
            _userCollection = database.GetCollection<사용자>("Users");
            _pkiService = pkiService;
        }

        // 사용자에 대한 X509 인증서 생성 및 저장
        [HttpPost("createCertificate")]
        public async Task<IActionResult> CreateCertificate([FromBody] 사용자 user)
        {
            try
            {
                // 서비스 클래스를 이용해 인증서 생성
                var certificate = _pkiService.CreateCertificate(user.Id, user.이름);

                // 사용자 정보에 인증서 추가
                user.공개키인증서 = certificate;
                user.생성일 = certificate.생성일;
                user.만료일 = certificate.만료일;

                // MongoDB에 사용자 저장
                await _userCollection.InsertOneAsync(user);

                return Ok(new { message = "인증서 생성 및 저장 성공", certificate = certificate });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "오류 발생", error = ex.Message });
            }
        }

        // 공개키 조회
        [HttpGet("{id}/getPublicKey")]
        public async Task<IActionResult> GetPublicKey(string id)
        {
            var user = await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null || user.공개키인증서 == null)
            {
                return NotFound(new { message = "사용자 또는 공개키 인증서가 없습니다." });
            }

            return Ok(new { publicKey = user.공개키인증서.공개키 });
        }

        // 인증서 전체 조회
        [HttpGet("{id}/getCertificate")]
        public async Task<IActionResult> GetCertificate(string id)
        {
            var user = await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null || user.공개키인증서 == null)
            {
                return NotFound(new { message = "사용자 또는 인증서를 찾을 수 없습니다." });
            }

            return Ok(user.공개키인증서);
        }

        // 인증서 갱신
        [HttpPost("{id}/renewCertificate")]
        public async Task<IActionResult> RenewCertificate(string id)
        {
            var user = await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null || user.공개키인증서 == null)
            {
                return NotFound(new { message = "사용자 또는 인증서를 찾을 수 없습니다." });
            }

            // 인증서 갱신
            var renewedCert = _pkiService.RenewCertificate(user.공개키인증서);
            user.공개키인증서 = renewedCert;
            // MongoDB에 갱신된 사용자 인증서 저장
            await _userCollection.ReplaceOneAsync(u => u.Id == id, user);

            return Ok(new { message = "인증서 갱신 성공", certificate = renewedCert });
        }

        // 인증서 폐지 (Revocation)
        [HttpPost("{id}/revokeCertificate")]
        public async Task<IActionResult> RevokeCertificate(string id)
        {
            var user = await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null || user.공개키인증서 == null)
            {
                return NotFound(new { message = "사용자 또는 인증서를 찾을 수 없습니다." });
            }

            // 인증서 폐지
            var revokedCert = new RevokedCertificate
            {
                SerialNumber = user.공개키인증서.인증서_일련번호,
                RevokedDate = DateTime.UtcNow
            };

            var database = _pkiService.GetRevokedCertificatesCollection();
            await database.InsertOneAsync(revokedCert);

            return Ok(new { message = "인증서 폐지 성공", revokedCertificate = revokedCert });
        }

        // 인증서 서명 검증
        [HttpPost("verifySignature")]
        public IActionResult VerifySignature([FromBody] VerifyRequest verifyRequest)
        {
            var user = _userCollection.Find(u => u.Id == verifyRequest.UserId).FirstOrDefault();
            if (user == null || user.공개키인증서 == null)
            {
                return NotFound(new { message = "사용자 또는 인증서를 찾을 수 없습니다." });
            }

            var isSignatureValid = _pkiService.VerifySignature(
                Convert.FromBase64String(verifyRequest.Data),
                Convert.FromBase64String(verifyRequest.Signature),
                user.공개키인증서.공개키
            );

            if (isSignatureValid)
            {
                return Ok(new { message = "서명 검증 성공" });
            }

            return BadRequest(new { message = "서명 검증 실패" });
        }

        // 인증서 폐지 여부 확인
        [HttpGet("isCertificateRevoked/{serialNumber}")]
        public async Task<IActionResult> IsCertificateRevoked(string serialNumber)
        {
            var isRevoked = await _pkiService.IsCertificateRevoked(serialNumber);

            if (isRevoked)
            {
                return Ok(new { message = "인증서는 폐지되었습니다." });
            }

            return Ok(new { message = "인증서는 유효합니다." });
        }
    }
}
