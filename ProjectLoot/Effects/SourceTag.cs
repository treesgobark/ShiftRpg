using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

[Flags]
public enum SourceTag
{
    None     = 0x_0000_0000,
    Gun      = 0x_0000_0001,
    Melee    = 0x_0000_0002,
    Shatter  = 0x_0000_0004,
    All      = 0x_7FFF_FFFF,
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