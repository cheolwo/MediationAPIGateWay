using MediatR;
using Microsoft.Extensions.Logging;
using 仁Common.Command;
using 仁Common.Services;

namespace 仁Common.Handlr
{
    public class ErrorCommandHandler : IRequestHandler<ErrorCommand, Unit>
    {
        private readonly ILogger<ErrorCommandHandler> _logger;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public ErrorCommandHandler(ILogger<ErrorCommandHandler> logger, IEmailService emailService, ISmsService smsService)
        {
            _logger = logger;
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task<Unit> Handle(ErrorCommand request, CancellationToken cancellationToken)
        {
            // 에러 정보를 로그로 남김
            _logger.LogError($"Error occurred for UserId: {request.UserId}, Error: {request.ErrorMessage}");

            // 이메일 알림 전송
            await _emailService.SendEmailAsync(request.UserId, "Error Notification", $"An error occurred: {request.ErrorMessage}");

            // SMS 알림 전송
            await _smsService.SendSmsAsync(request.UserId, $"An error occurred: {request.ErrorMessage}");

            return Unit.Value;
        }
    }
}
