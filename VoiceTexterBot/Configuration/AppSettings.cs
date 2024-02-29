using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTexterBot.Configuration;

internal class AppSettings
{
    public string BotToken { get; set; }

    public string DownloadsFolder { get; set; }

    public string AudiofileName { get; set; }

    public string InputAudioFormat { get; set; }

    public string OutputAudioFormat { get; set; }

    public float InputAudioBitrate { get; set; }
}
