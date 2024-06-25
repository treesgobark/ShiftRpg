namespace ProjectLoot.Effects;

public class HitstunEffect
{
    public Team AppliesTo { get; }
    public SourceTag Source { get; }
    public TimeSpan Duration { get; }
    
    public HitstunEffect(Team appliesTo, SourceTag source, TimeSpan duration)
    {
        AppliesTo = appliesTo;
        Source = source;
        Duration = duration;
    }
}
