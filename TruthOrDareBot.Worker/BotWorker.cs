using Telegram.Bot;
using TruthOrDareBot.Game.Entities;
using TruthOrDareBot.Game.Extensions;
using TruthOrDareBot.Game.Manager;

namespace TruthOrDareBot.Worker;

public class BotWorker(
    IConfiguration config,
    ContentProvider content,
    ILogger<BotWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var token = config["Telegram:Token"]
            ?? throw new InvalidOperationException("Telegram:Token não configurado.");

        var bot = new TelegramBotClient(token, cancellationToken: stoppingToken);
        var me = await bot.GetMe(stoppingToken);

        bot.OnError += (ex, source) =>
        {
            logger.LogError(ex, "Erro no bot ({Source})", source);
            return Task.CompletedTask;
        };

        bot.OnMessage += async (msg, type) =>
        {
            if (msg.Text is null) return;

            // em grupos o comando chega como "/comando@NomeDoBot"
            var command = msg.Text.Split(' ')[0].Split('@')[0].ToLowerInvariant();

            if (command == "/help")
            {
                await bot.SendMessage(msg.Chat, HelpText);
                return;
            }

            if (RaceCommands.TryGetValue(command, out var race))
                await bot.SendMessage(msg.Chat, DrawRandom(race));
        };

        logger.LogInformation("@{Username} rodando", me.Username);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private string DrawRandom(Race race)
    {
        var choice = Random.Shared.Next(2) == 0 ? Choice.Truth : Choice.Dare;
        var phrase = content.Draw(race, choice);

        if (phrase is null)
        {
            choice = choice == Choice.Truth ? Choice.Dare : Choice.Truth;
            phrase = content.Draw(race, choice);
        }

        return phrase is null
            ? $"Ainda não há verdades nem desafios cadastrados para {race.Label()}."
            : $"{choice.Label()}: {phrase}";
    }

    private static readonly Dictionary<string, Race> RaceCommands =
        Enum.GetValues<Race>().ToDictionary(race => $"/jogar{race.CommandName()}");

    private static readonly string HelpText = BuildHelp();

    private static string BuildHelp()
    {
        var lines = Enum.GetValues<Race>()
            .Select(race => $"/jogar{race.CommandName()}: {race.Label()}");

        return "Verdade ou Desafio\n\n" +
               "Use o comando da sua raça e o bot sorteia uma verdade ou um desafio:\n\n" +
               string.Join("\n", lines) +
               "\n\n/help: mostra esta mensagem";
    }
}