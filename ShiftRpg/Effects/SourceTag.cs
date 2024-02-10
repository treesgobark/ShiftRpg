using System;

namespace ShiftRpg.Effects;

[Flags]
public enum SourceTag
{
    Gun   = 0x_0000_0001,
    Melee = 0x_0000_0002,
}

public static class SourceTagExtensions
{
    public static bool IsSubsetOf(this SourceTag subset, SourceTag superset)
    {
        return (subset & superset) == subset;
    }
}