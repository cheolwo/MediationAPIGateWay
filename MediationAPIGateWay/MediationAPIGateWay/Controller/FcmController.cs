using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text;
using System.Text.Json;

namespace MediationAPIGateWay.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FcmController : ControllerBase
    {
        private readonly ILogger<FcmController> _logger;
        private readonly IMongoCollection<사용자토큰> _userTokenCollection;
        private readonly IConfiguration _configuration;

        public FcmController(ILogger<FcmController> logger, IMongoDatabase database, IConfiguration configuration)
        {
            _logger = logger;
            _userTokenCollection = database.GetCollection<사용자토큰>("사용자토큰");
            _configuration = configuration;
        }

        // FCM 토큰 등록 API
        [HttpPost("register")]
        public async Task<IActionResult> RegisterToken([FromBody] FcmTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required");
            }

            var 사용자Id = "exampleUserId"; // 예시로 고정된 사용자 ID 사용

            var existingToken = await _userTokenCollection.Find(t => t.FcmToken == request.Token && t.사용자Id == 사용자Id).FirstOrDefaultAsync();
            if (existingToken != null)
            {
                return Ok("Token already registered");
            }

            var userToken = new 사용자토큰
            {
                Id = Guid.NewGuid().ToString(),
                사용자Id = 사용자Id,
                FcmToken = request.Token,
                생성일 = DateTime.UtcNow,
                만료일 = DateTime.UtcNow.AddMonths(6) // 6개월 만료
            };

            await _userTokenCollection.InsertOneAsync(userToken);
            _logger.LogInformation($"FCM Token registered for user {사용자Id}: {request.Token}");
            return Ok();
        }

        // 푸시 알림 전송 API
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notification)
        {
            var tokens = await _userTokenCollection.Find(t => t.만료일 > DateTime.UtcNow).ToListAsync();

            foreach (var token in tokens)
            {
                await SendPushNotification(token.FcmToken, notification.Title, notification.Message);
            }

            return Ok("Notifications sent");
        }

        private async Task SendPushNotification(string token, string title, string message)
        {
            var serverKey = _configuration["FcmSettings:ServerKey"];
            var senderId = _configuration["FcmSettings:SenderId"];

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + serverKey);

            var notificationObject = new
            {
                to = token,
                notification = new
                {
                    title = title,
                    body = message
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(notificationObject), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", content);
            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Push Notification sent to {token}: {responseString}");
        }
    }

    // FCM 토큰 요청 모델
    public class FcmTokenRequest
    {
        public string Token { get; set; }
    }

    // 알림 전송 요청 모델
    public class NotificationRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
