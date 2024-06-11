using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record ApplyShatterEffect(Team AppliesTo, SourceTag Source) : IEffect
{
    public Guid EffectId { get; } = Guid.NewGuid();
}