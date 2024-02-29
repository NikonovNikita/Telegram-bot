using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTexterBot.Services;

internal interface IFileHandler
{
    Task Download(string fileId, CancellationToken ct);

    string Process(string inputParam);
}
