using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KikonovBot.Controllers;

internal class DefaultMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    
    public DefaultMessageController(ITelegramBotClient telegramClient)
    {
        _telegramClient = telegramClient;
    }

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Неподдерживаемый тип сообщений :(",
            cancellationToken: cancellationToken);
    }
}
