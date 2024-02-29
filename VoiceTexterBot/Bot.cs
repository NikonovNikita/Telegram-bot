using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBot.Controllers;

namespace VoiceTexterBot;

internal class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;

    private InlineKeyboardController _inlineKeyboardController;
    private DefaultMessageController _defaultMessageController;
    private TextMessageController _textMessageController;
    private VoiceMessageController _voiceMessageController;

    public Bot(ITelegramBotClient telegramClient,
        InlineKeyboardController inlineKeyboardController,
        TextMessageController textMessageController,
        DefaultMessageController defaultMessageController,
        VoiceMessageController voiceMessageController)
    {
        _telegramClient = telegramClient;
        _inlineKeyboardController = inlineKeyboardController;
        _textMessageController = textMessageController;
        _defaultMessageController = defaultMessageController;
        _voiceMessageController = voiceMessageController;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new Telegram.Bot.Polling.ReceiverOptions()
        {
            AllowedUpdates = { }
        }, 
        cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        if(update.Type == UpdateType.Message)
        {
            switch(update.Message!.Type)
            {
                case MessageType.Voice:
                    await _voiceMessageController.Handle(update.Message!, cancellationToken);
                    return;
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message!, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message!, cancellationToken);
                    return;
            }
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n" +
            $"{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением. . .");
        
        Thread.Sleep(10000);

        return Task.CompletedTask;
    }
}
