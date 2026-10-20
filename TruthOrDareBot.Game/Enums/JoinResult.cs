using System.Runtime.Serialization;

namespace TruthOrDareBot.Game.Enums;

public enum JoinResult
{
    [EnumMember(Value = "Unchange")] Unchange,
    [EnumMember(Value = "RaceChanged")] RaceChanged,
    [EnumMember(Value = "Joined")] Joined,
    [EnumMember(Value = "Closed")] Closed
}