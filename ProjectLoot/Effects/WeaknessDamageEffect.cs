using System.Collections.Generic;

namespace ProjectLoot.Effects;

public record WeaknessDamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float WeaknessDamage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases)
{
    public WeaknessDamageEffect(Team AppliesTo, SourceTag Source, float WeaknessDamage)
        : this(AppliesTo, Source, WeaknessDamage, new List<float>(), new List<float>())
    {
        
    }
    
    public Guid EffectId { get; } = Guid.NewGuid();
}