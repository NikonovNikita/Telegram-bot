using KikonovBot.Configuration;
using KikonovBot.Controllers;
using KikonovBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

namespace KikonovBot;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext,  services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();
        
        Console.WriteLine("Сервис запущен");

        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static AppConfig BuildConfig()
    {
        return new AppConfig()
        {
            BotToken = "6782834133:AAHPRfWOLS31uutAtqZw-jlvTEUIgJMPjVM"
        };
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ILogic, BusinessLogic>();
        
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<TextMessageController>();
        
        services.AddSingleton<IStorage, SessionStorage>();
        
        AppConfig appConfig = BuildConfig();
        services.AddSingleton(appConfig);
        
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appConfig.BotToken));

        services.AddHostedService<Bot>();
    }
}
