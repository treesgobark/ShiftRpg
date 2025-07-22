using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class DeathEffect : IEffect
{
    public DeathEffect(Team appliesTo, SourceTag source)
    {
        AppliesTo = appliesTo;
        Source    = source;
    }

    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
}