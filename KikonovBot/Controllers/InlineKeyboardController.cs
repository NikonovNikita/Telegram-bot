using KikonovBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KikonovBot.Controllers;

internal class InlineKeyboardController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _sessionStorage;
    private readonly ILogic _businessLogic;

    public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage sessionStorage, ILogic businessLogic)
    {
        _telegramClient = telegramClient;
        _sessionStorage = sessionStorage;
        _businessLogic = businessLogic;
    }

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery?.Data == null)
            return;

        _sessionStorage.GetUserSession(callbackQuery.From.Id).UserSelection = callbackQuery.Data;

        switch (callbackQuery.Data)
        {
            case "сумма чисел":
                _sessionStorage.GetUserSession(callbackQuery.From.Id).functionDelegate = _businessLogic.Sum;
                break;
            case "подсчет символов":
                _sessionStorage.GetUserSession(callbackQuery.From.Id).functionDelegate = _businessLogic.Counting;
                break;
        }

        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"В чате действует функция: <b>{callbackQuery.Data}</b>",
            cancellationToken: cancellationToken, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
    }
}
