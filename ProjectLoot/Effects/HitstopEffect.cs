using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class HitstopEffect : IEffect
{
    public Team AppliesTo { get; }
    public SourceTag Source { get; }
    public TimeSpan Duration { get; }

    public HitstopEffect(Team appliesTo, SourceTag source, TimeSpan duration)
    {
        AppliesTo = appliesTo;
        Source = source;
        Duration = duration;
    }
}