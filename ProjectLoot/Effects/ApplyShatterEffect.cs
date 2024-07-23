namespace ProjectLoot.Effects;

public class ApplyShatterEffect
{
    public ApplyShatterEffect(Team appliesTo, SourceTag source)
    {
        AppliesTo = appliesTo;
        Source    = source;
    }

    public Guid EffectId { get; } = Guid.NewGuid();
    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }

    public void Deconstruct(out Team appliesTo, out SourceTag source)
    {
        appliesTo = AppliesTo;
        source    = Source;
    }
}