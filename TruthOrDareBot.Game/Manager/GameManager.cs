using System.Collections.Concurrent;

namespace TruthOrDareBot.Game;

public class GameManager
{
    private readonly ConcurrentDictionary<long, GameSession> _sessions = new();

    public GameSession Create(long chatId, long hostId)
    {
        var session = new GameSession(hostId);
        _sessions[chatId] = session;
        return session;
    }

    public GameSession? Get(long chatId) =>
        _sessions.TryGetValue(chatId, out var s) ? s : null;

    public void Remove(long chatId) => _sessions.TryRemove(chatId, out _);
}