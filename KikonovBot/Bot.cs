using KikonovBot.Controllers;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KikonovBot;

internal class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private TextMessageController _textMessageController;
    private InlineKeyboardController _inlineKeyboardController;
    private DefaultMessageController _defaultMessageController;
    
    public Bot(ITelegramBotClient telegramClient,
        TextMessageController textMessageController,
        InlineKeyboardController inlineKeyboardController,
        DefaultMessageController defaultMessageController)
    {
        _telegramClient = telegramClient;
        _textMessageController = textMessageController;
        _inlineKeyboardController = inlineKeyboardController;
        _defaultMessageController = defaultMessageController;
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
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
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

        Console.WriteLine($"Error: {errorMessage}");

        Console.WriteLine("Ожидание 10 секунд перед повторным подключением. . .");

        Thread.Sleep(10000);

        return Task.CompletedTask;
    }
}
