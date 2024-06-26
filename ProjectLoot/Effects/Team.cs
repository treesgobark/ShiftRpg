namespace ProjectLoot.Effects;

[Flags]
public enum Team
{
    Player = 0b_0000_0001,
    Enemy  = 0b_0000_0010,
}

public static class TeamExtensions
{
    public static bool IsSubsetOf(this Team subset, Team superset)
    {
        return (subset & superset) == subset;
    }
    
    public static bool Contains(this Team superset, Team subset)
    {
        return (subset & superset) == subset;
    }
}