using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;

namespace VoiceTexterBot;

class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Сервис запущен");

        await host.RunAsync();
        
        Console.WriteLine("Сервис остановлен");
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "6951399570:AAF5_JI6tIe3DLK6Ytk8ZaI6rj8wPuHa4l4",
            DownloadsFolder = "C:\\Users\\Никонов\\Downloads",
            AudiofileName = "audio",
            InputAudioFormat = "ogg",
            OutputAudioFormat = "wav",
            InputAudioBitrate = 48000
        };

    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IFileHandler, AudiofileHandler>();
        
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);
        
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<VoiceMessageController>();

        services.AddSingleton<IStorage, MemoryStorage>();
        
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        
        services.AddHostedService<Bot>();
    }
}
