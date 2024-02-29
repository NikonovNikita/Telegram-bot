using KikonovBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikonovBot.Services;

internal interface IStorage
{
    internal UserSession GetUserSession(long userId);
}
