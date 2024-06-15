using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record ApplyShatterEffect(Team AppliesTo, SourceTag Source) : IEffect
{
    public Guid EffectId { get; } = Guid.NewGuid();
}