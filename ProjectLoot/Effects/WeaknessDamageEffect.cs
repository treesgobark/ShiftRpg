using System.Collections.Generic;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class WeaknessDamageEffect : IEffect
{
    public WeaknessDamageEffect(Team appliesTo, SourceTag source, float weaknessDamage)
        : this(appliesTo, source, weaknessDamage, new List<float>(), new List<float>())
    {
        
    }

    public WeaknessDamageEffect(Team               appliesTo,
                                SourceTag          source,
                                float              weaknessDamage,
                                ICollection<float> additiveIncreases,
                                ICollection<float> multiplicativeIncreases)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        WeaknessDamage          = weaknessDamage;
        AdditiveIncreases       = additiveIncreases;
        MultiplicativeIncreases = multiplicativeIncreases;
    }

    public Guid EffectId { get; } = Guid.NewGuid();
    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float WeaknessDamage { get; init; }
    public ICollection<float> AdditiveIncreases { get; init; }
    public ICollection<float> MultiplicativeIncreases { get; init; }

    public void Deconstruct(out Team           appliesTo, out SourceTag      source, out float          weaknessDamage, out ICollection<float> additiveIncreases, out ICollection<float> multiplicativeIncreases)
    {
        appliesTo               = AppliesTo;
        source                  = Source;
        weaknessDamage          = WeaknessDamage;
        additiveIncreases       = AdditiveIncreases;
        multiplicativeIncreases = MultiplicativeIncreases;
    }
}