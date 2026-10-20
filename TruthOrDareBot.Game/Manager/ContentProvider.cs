using System.Text.Json;
using TruthOrDareBot.Game.Entities;
using TruthOrDareBot.Game.Extensions;

namespace TruthOrDareBot.Game.Manager;

public class ContentPool
{
    public List<string> Truths { get; set; } = new();
    public List<string> Dares { get; set; } = new();
}

public class ContentProvider
{
    private readonly Dictionary<string, ContentPool> _pools;

    public ContentProvider(string path)
    {
        var json = File.ReadAllText(path);
        _pools = JsonSerializer.Deserialize<Dictionary<string, ContentPool>>(
                     json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                 ?? throw new InvalidOperationException("content.json inválido.");
    }
    
    public string? Draw(Race race, Choice choice)
    {
        var options = new[] { race.ToString().ToLowerInvariant(), "common" }
            .Select(GetPool)
            .Where(pool => pool is not null)
            .SelectMany(pool => choice == Choice.Truth ? pool!.Truths : pool!.Dares)
            .ToList();

        return options.Count == 0 ? null : options[Random.Shared.Next(options.Count)];
    }

    public string? Draw(Player player, Choice choice) => Draw(player.Race, choice);

    public ContentPool? GetPool(string name) =>
        _pools.TryGetValue(name, out var pool) ? pool : null;
}