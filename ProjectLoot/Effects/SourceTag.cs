using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

[Flags]
public enum SourceTag
{
    None    = 0x_0000_0000,
    Gun     = 0x_0000_0001,
    Melee   = 0x_0000_0002,
    Shatter = 0x_0000_0004,
    Sword   = 0x_0000_000A, // 0x_0000_0008 + 0x_0000_0002
    Fists   = 0x_0000_0012, // 0x_0000_0010 + 0x_0000_0002
    Dagger  = 0x_0000_0022, // 0x_0000_0020 + 0x_0000_0002
    Pistol  = 0x_0000_0041, // 0x_0000_0040 + 0x_0000_0001
    Rifle   = 0x_0000_0081, // 0x_0000_0080 + 0x_0000_0001
    Shotgun = 0x_0000_0101, // 0x_0000_0100 + 0x_0000_0001
    Spear   = 0x_0000_0202, // 0x_0000_0200 + 0x_0000_0002
    All     = 0x_7FFF_FFFF,
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
    
    public static bool MatchesRequirements(this SourceTag source, SourceTag requirements, MatchingStrategy matchingStrategy)
    {
        return matchingStrategy switch
        {
            MatchingStrategy.Any => (requirements & source) > 0,
            MatchingStrategy.Exact => requirements == source,
            MatchingStrategy.Subset => source.IsContainedIn(requirements),
            _ => throw new ArgumentOutOfRangeException(nameof(matchingStrategy), matchingStrategy, null)
        };
    }
}