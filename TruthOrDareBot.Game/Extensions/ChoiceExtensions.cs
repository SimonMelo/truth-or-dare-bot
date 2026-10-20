namespace TruthOrDareBot.Game.Extensions;

public enum Choice { Truth, Dare }

public static class ChoiceExtensions
{
    public static string Label(this Choice choice) =>
        choice == Choice.Truth ? "Verdade" : "Desafio";
}