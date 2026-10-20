public enum Race { Vampire, Witch, Werewolf, Seelie }

public static class RaceExtensions
{
    public static string Label(this Race race) => race switch
    {
        // Esse bot foi adaptado para um RPG no Telegram envolvendo raças.
        // Adapte da forma que preferir.

        // Exemplo:
        Race.Vampire  => "🧛 Vampiro",
        Race.Witch    => "🧙‍♀️ Bruxa",
        Race.Werewolf => "🐺 Lobisomem",
        Race.Seelie   => "🧚 Seelie",
        _ => race.ToString()
    };
    
    public static string CommandName(this Race race) => race switch
    {
        // Esse bot foi adaptado para um RPG no Telegram envolvendo raças.
        // Adapte da forma que preferir.

        // Exemplo:
        Race.Vampire  => "vampiro",
        Race.Witch    => "bruxa",
        Race.Werewolf => "lobisomem",
        Race.Seelie   => "seelie",
        _ => race.ToString().ToLowerInvariant()
    };
}