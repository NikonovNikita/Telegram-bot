using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Utilities;

namespace VoiceTexterBot.Services;

internal class AudiofileHandler : IFileHandler
{
    private readonly AppSettings _appSettings;
    private readonly ITelegramBotClient _telegramClient;

    public AudiofileHandler(ITelegramBotClient telegramClient, AppSettings appSettings)
    {
        _telegramClient = telegramClient;
        _appSettings = appSettings;
    }
    public async Task Download(string fileId, CancellationToken ct)
    {
        string inputAudiofilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudiofileName}." +
            $"{_appSettings.InputAudioFormat}");

        using(FileStream destinationStream = File.Create(inputAudiofilePath))
        {
            var file = await _telegramClient.GetFileAsync(fileId, ct);
            
            if (file.FilePath == null)
                return;

            await _telegramClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
        }
    }

    public string Process(string languageCode)
    {
        string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudiofileName}." +
            $"{_appSettings.InputAudioFormat}");
        string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudiofileName}." +
            $"{_appSettings.OutputAudioFormat}");

        Console.WriteLine("Начинаем конвертацию. . .");
        AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
        Console.WriteLine("Файл конвертирован");

        Console.WriteLine("Начинаем распознавание. . .");
        var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.InputAudioBitrate, languageCode);
        Console.WriteLine("Файл распознан");

        return speechText;
    }
}
