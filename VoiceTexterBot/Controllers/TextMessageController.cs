using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace VoiceTexterBot.Controllers;

internal class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public TextMessageController(ITelegramBotClient telegramClient)
    {
        _telegramClient = telegramClient;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($" Русский", $"ru"),
                    InlineKeyboardButton.WithCallbackData($" English", $"en"),
                    InlineKeyboardButton.WithCallbackData($" Español", $"es")
                });

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот превращает аудио в текст.</b>" +
                    $"{Environment.NewLine}{Environment.NewLine}Можно записать сообщение и переслать другу, если " +
                    $"лень печатать. {Environment.NewLine}", cancellationToken: ct, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
                break;

            default:
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте аудио для превращения в текст.",
                    cancellationToken: ct);
                break;
        }
    }
}