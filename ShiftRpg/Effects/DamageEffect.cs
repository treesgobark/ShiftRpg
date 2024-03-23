using System;
using System.Collections.Generic;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record DamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float Damage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases) : IEffect
{
    public DamageEffect(Team AppliesTo, SourceTag Source, float Damage)
        : this(AppliesTo, Source, Damage, new List<float>(), new List<float>()) { }

    public Guid EffectId { get; } = Guid.NewGuid();
}