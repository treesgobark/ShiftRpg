using System;
using System.Collections.Generic;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

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