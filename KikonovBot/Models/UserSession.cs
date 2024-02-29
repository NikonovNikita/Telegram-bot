using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KikonovBot.Models;

internal class UserSession
{
    internal delegate int FunctionDelegate(string value);
    internal string UserSelection {  get; set; }
    internal FunctionDelegate functionDelegate { get; set; }
}
