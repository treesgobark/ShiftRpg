using System.Collections.Generic;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record WeaknessDamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float WeaknessDamage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases) : IEffect
{
    public WeaknessDamageEffect(Team AppliesTo, SourceTag Source, float WeaknessDamage)
        : this(AppliesTo, Source, WeaknessDamage, new List<float>(), new List<float>())
    {
        
    }
    
    public Guid EffectId { get; } = Guid.NewGuid();
}