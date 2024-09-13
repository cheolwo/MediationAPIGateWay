using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace 텔레그램Infra
{
    public class TelegramManager
    {
        private readonly TelegramBotClient _botClient;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public TelegramManager(string botToken)
        {
            _botClient = new TelegramBotClient(botToken);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        // 채팅방 생성 메서드 (텔레그램 API에서는 직접 그룹 채팅을 생성하는 메서드를 제공하지 않음)
        // 사용자가 봇과 상호작용을 통해 그룹을 생성해야 함
        public async Task SendStartMessageAsync(string chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message); // HTML 파서 사용
        }

        // 초대 링크 생성 (그룹에 사용자 초대 시 사용)
        public async Task<string> CreateInviteLinkAsync(long chatId)
        {
            var inviteLink = await _botClient.CreateChatInviteLinkAsync(chatId);
            return inviteLink.InviteLink;
        }

        // 사용자 초대 메서드 (사용자에게 초대 링크를 보내는 방식)
        public async Task InviteUsersToGroupAsync(long chatId, List<string> userPhoneNumbers)
        {
            var inviteLink = await CreateInviteLinkAsync(chatId); // 초대 링크 생성

            // 각 사용자에게 초대 링크를 발송 (여기서는 전화번호로 직접 초대는 불가하므로 메시지로 초대 링크를 공유합니다.)
            foreach (var phoneNumber in userPhoneNumbers)
            {
                await SendMessageAsync(chatId, $"Please join using this invite link: {inviteLink}");
            }
        }

        // 메시지 전송 메서드
        public async Task SendMessageAsync(long chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message); // ParseMode 사용
        }

        // 메시지 핸들링 메서드
        public async Task HandleUpdateAsync(Update update)
        {
            if (update.Message?.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var message = update.Message.Text;

                // 봇의 기본 응답 예시
                await SendMessageAsync(chatId, $"You said: {message}");
            }
        }
        // 봇 종료
        public void StopReceiving()
        {
            _cancellationTokenSource.Cancel(); // 수신을 중단
        }
    }
}
