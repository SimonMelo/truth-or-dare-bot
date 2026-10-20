using TruthOrDareBot.Game.Entities;

namespace TruthOrDareBot.Game;

public enum GameState { Lobby, Playing }
public enum JoinResult { Joined, RaceChanged, Unchanged, Closed }

public class GameSession(long hostId)
{
    private const int MinPlayers = 1;

    private readonly List<Player> _players = new();
    private readonly object _lock = new();
    private int _current;

    public long HostId { get; } = hostId;
    public GameState State { get; private set; } = GameState.Lobby;

    public IReadOnlyList<Player> Players
    {
        get { lock (_lock) return _players.ToList(); }
    }

    public Player? CurrentPlayer
    {
        get
        {
            lock (_lock)
                return State == GameState.Playing && _players.Count > 0
                    ? _players[_current]
                    : null;
        }
    }

    public JoinResult Join(Player player)
    {
        lock (_lock)
        {
            if (State != GameState.Lobby) return JoinResult.Closed;

            var i = _players.FindIndex(p => p.Id == player.Id);
            if (i < 0) { _players.Add(player); return JoinResult.Joined; }
            if (_players[i].Race == player.Race) return JoinResult.Unchanged;
            _players[i] = player;
            return JoinResult.RaceChanged;
        }
    }

    public bool Start()
    {
        lock (_lock)
        {
            if (State != GameState.Lobby || _players.Count < MinPlayers) return false;
            State = GameState.Playing;
            _current = Random.Shared.Next(_players.Count); // quem começa é sorteado
            return true;
        }
    }

    public void NextTurn()
    {
        lock (_lock) _current = (_current + 1) % _players.Count;
    }
}