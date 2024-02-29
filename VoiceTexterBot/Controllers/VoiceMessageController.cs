using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers;

internal class VoiceMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly AppSettings _appSettings;
    private readonly IFileHandler _audiofileHandler;
    private readonly IStorage _memoryStorage;

    public VoiceMessageController(ITelegramBotClient telegramClient, AppSettings appSettings, IFileHandler audiofileHandler,
        IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _appSettings = appSettings;
        _audiofileHandler = audiofileHandler;
        _memoryStorage = memoryStorage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        var fileId = message.Voice?.FileId;

        if (fileId == null)
            return;

        await _audiofileHandler.Download(fileId, ct);

        string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;
        Console.WriteLine($"Язык сессии пользователя: {userLanguageCode}");

        var result = _audiofileHandler.Process(userLanguageCode);

        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>{result}</b>", cancellationToken: ct, parseMode:
            Telegram.Bot.Types.Enums.ParseMode.Html);
    }
}
