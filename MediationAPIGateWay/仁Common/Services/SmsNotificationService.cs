using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace 仁Common.Services
{
    public class SmsNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsNotificationService> _logger;

        public SmsNotificationService(IConfiguration configuration, ILogger<SmsNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task NotifyUserAsync(string userId, string message)
        {
            // 사용자의 전화번호를 조회 (여기서는 가정)
            string userPhoneNumber = GetUserPhoneNumberById(userId);

            if (!string.IsNullOrEmpty(userPhoneNumber))
            {
                // SMS 전송 로직 (SMS API 사용)
                await SendSmsAsync(userPhoneNumber, message);
                _logger.LogInformation($"SMS notification sent to user {userId} at {userPhoneNumber}");
            }
            else
            {
                _logger.LogWarning($"No phone number found for user {userId}");
            }
        }

        public async Task NotifyAdminAsync(string message)
        {
            // 관리자 전화번호를 구성 파일에서 가져옴
            string adminPhoneNumber = _configuration["AdminPhoneNumber"];
            await SendSmsAsync(adminPhoneNumber, message);
            _logger.LogInformation($"Admin notified at {adminPhoneNumber}");
        }

        private string GetUserPhoneNumberById(string userId)
        {
            // 사용자 ID를 통해 전화번호를 조회하는 로직 (예: DB 또는 캐시에서 조회)
            // 여기서는 간단히 하드코딩된 값을 반환
            return "010-1234-5678";
        }

        private async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            // SMS 전송 로직 (SMS API 사용)
            _logger.LogInformation($"Sending SMS to {toPhoneNumber} with message: {message}");

            // 실제 SMS 전송 코드는 여기에서 구현
            await Task.CompletedTask;
        }
    }
}
