using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record ApplyShatterEffect(Team AppliesTo, SourceTag Source)
{
    public Guid EffectId { get; } = Guid.NewGuid();
}