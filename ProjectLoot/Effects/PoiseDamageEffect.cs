using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class PoiseDamageEffect : IEffect
{
    public Team AppliesTo { get; }
    public SourceTag Source { get; }
    public float PoiseDamage { get; }

    public PoiseDamageEffect(Team appliesTo, SourceTag source, float poiseDamage)
    {
        AppliesTo   = appliesTo;
        Source      = source;
        PoiseDamage = poiseDamage;
    }
}