using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7510774105:AAE9QYB71q5NFUMIfR1kdAnVnZ_0r2-_ibM");

var me = await bot.GetMeAsync();
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");

bot.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: new ReceiverOptions
    {
        AllowedUpdates = Array.Empty<UpdateType>() // 받으려는 업데이트 유형 설정 (빈 배열은 모든 업데이트 받음)
    },
    cancellationToken: cts.Token
);

Console.ReadLine();
cts.Cancel(); // stop the bot

// 메시지 수신 시 처리하는 메서드
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // 텍스트 메시지일 경우만 처리
    if (update.Type == UpdateType.Message && update.Message?.Text != null)
    {
        var message = update.Message;
        Console.WriteLine($"Received a '{message.Text}' message in chat {message.Chat.Id}");

        // 사용자가 보낸 메시지에 대해 답장
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"{message.From.FirstName} said: {message.Text}",
            cancellationToken: cancellationToken
        );
    }
}

// 에러 발생 시 처리하는 메서드
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    // 에러 로그 출력
    Console.WriteLine(exception);
    return Task.CompletedTask;
}
