using System.Collections.Generic;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record DamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float Value,
    List<float> AdditiveIncreases,
    List<float> MultiplicativeIncreases) : INumericalEffect<float>
{
    public DamageEffect(Team AppliesTo, SourceTag Source, float Value)
        : this(AppliesTo, Source, Value, new List<float>(), new List<float>()) { }

    public Guid EffectId { get; } = Guid.NewGuid();
}