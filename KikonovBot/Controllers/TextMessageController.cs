using KikonovBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KikonovBot.Controllers;

internal class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _sessionStorage;

    public TextMessageController(ITelegramBotClient telegramClient, IStorage sessionStorage)
    {
        _telegramClient = telegramClient;
        _sessionStorage = sessionStorage;
    }

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "/start":
                
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел", $"сумма чисел"),
                        InlineKeyboardButton.WithCallbackData($" Подсчет символов", $"подсчет символов")
                    }
                };

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Hola! Этот бот умеет складывать" +
                    $" числа и считать символы.</b>{Environment.NewLine}{Environment.NewLine}Выберите одну из " +
                    $"функций:", cancellationToken: cancellationToken, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
                
                break;

            default:
                if (_sessionStorage.GetUserSession(message.Chat.Id).UserSelection == null)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Выберите функцию",
                        cancellationToken: cancellationToken);
                if (_sessionStorage.GetUserSession(message.Chat.Id).UserSelection != null)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                        $"Результат: <b>{_sessionStorage.GetUserSession(message.Chat.Id).functionDelegate(message.Text!)}</b>",
                        cancellationToken: cancellationToken, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                break;
                    



                
                
                

        }
    }
}
