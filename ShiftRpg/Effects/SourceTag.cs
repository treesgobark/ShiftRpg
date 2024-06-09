using System;

namespace ShiftRpg.Effects;

[Flags]
public enum SourceTag
{
    Gun      = 0x_0000_0001,
    Melee    = 0x_0000_0002,
    Shatter  = 0x_0000_0004,
}

public static class SourceTagExtensions
{
    public static bool IsContainedIn(this SourceTag subset, SourceTag superset)
    {
        return (subset & superset) == subset;
    }
    
    public static bool Contains(this SourceTag superset, SourceTag subset)
    {
        return (subset & superset) == subset;
    }
}