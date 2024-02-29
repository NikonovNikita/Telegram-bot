using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers;

internal class InlineKeyboardController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _memoryStorage = memoryStorage;
    }

    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if(callbackQuery?.Data == null)
            return;

        _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

        string languageText = callbackQuery.Data switch
        {
            "ru" => " Русский",
            "en" => " Английский",
            "es" => " Испанский",
            _ => string.Empty
        };

        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
            $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>{Environment.NewLine}" +
            $"Запишите боту голосовое сообщение.", cancellationToken: ct, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
    }
}
