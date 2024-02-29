using KikonovBot.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikonovBot.Services;

internal class SessionStorage : IStorage
{
    private readonly ConcurrentDictionary<long, UserSession> _userSessions;
    
    public SessionStorage()
    {
        _userSessions = new ConcurrentDictionary<long, UserSession>();
    }

    public UserSession GetUserSession(long userId)
    {
        if(_userSessions.ContainsKey(userId))
            return _userSessions[userId];

        var newUserSession = new UserSession() { UserSelection = null };
        _userSessions.TryAdd(userId, newUserSession);
        return newUserSession;
    }
}
