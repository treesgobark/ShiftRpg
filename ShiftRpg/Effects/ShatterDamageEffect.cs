using System.Collections.Generic;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record ShatterDamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float ShatterDamage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases) : IEffect
{
    public ShatterDamageEffect(Team AppliesTo, SourceTag Source, float ShatterDamage)
        : this(AppliesTo, Source, ShatterDamage, new List<float>(), new List<float>())
    {
        
    }
    
    public Guid EffectId { get; } = Guid.NewGuid();
}