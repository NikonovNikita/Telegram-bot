using System.Collections.Concurrent;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services;

internal interface IStorage
{
    Session GetSession(long chatId);
}
