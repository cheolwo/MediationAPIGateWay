using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace 仁Common.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfiguration configuration, ILogger<EmailNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task NotifyUserAsync(string userId, string message)
        {
            // 사용자의 이메일 주소를 조회 (여기서는 가정)
            string userEmail = GetUserEmailById(userId);

            if (!string.IsNullOrEmpty(userEmail))
            {
                // 이메일 전송 로직 (SMTP 또는 다른 메일 전송 서비스 사용)
                await SendEmailAsync(userEmail, "Order Processing Error", message);
                _logger.LogInformation($"Notification sent to user {userId} at {userEmail}");
            }
            else
            {
                _logger.LogWarning($"No email found for user {userId}");
            }
        }

        public async Task NotifyAdminAsync(string message)
        {
            // 관리자 이메일 주소를 구성 파일에서 가져옴
            string adminEmail = _configuration["AdminEmail"];
            await SendEmailAsync(adminEmail, "System Alert: Order Processing Error", message);
            _logger.LogInformation($"Admin notified at {adminEmail}");
        }

        private string GetUserEmailById(string userId)
        {
            // 사용자 ID를 통해 이메일 주소를 조회하는 로직 (예: DB 또는 캐시에서 조회)
            // 여기서는 간단히 하드코딩된 값을 반환
            return "user@example.com";
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // 이메일 전송 로직 (SMTP 또는 이메일 API 사용)
            _logger.LogInformation($"Sending email to {toEmail} with subject: {subject}");

            // 실제 이메일 전송 코드는 여기에서 구현
            await Task.CompletedTask;
        }
    }
}
