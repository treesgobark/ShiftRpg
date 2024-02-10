using System;
using System.Collections.Generic;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record DamageEffect(
    Team AppliesTo,
    SourceTag Source,
    Guid EffectId,
    int Damage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases) : IEffect
{
    public DamageEffect(Team AppliesTo, SourceTag Source, Guid EffectId, int Damage)
        : this(AppliesTo, Source, EffectId, Damage, new List<float>(), new List<float>()) { }
}