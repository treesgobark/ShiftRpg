using System.Collections.Generic;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class ShatterDamageEffect : IEffect
{
    public ShatterDamageEffect(Team appliesTo, SourceTag source, float shatterDamage)
        : this(appliesTo, source, shatterDamage, new List<float>(), new List<float>())
    {
        
    }

    public ShatterDamageEffect(Team               appliesTo,
                               SourceTag          source,
                               float              shatterDamage,
                               ICollection<float> additiveIncreases,
                               ICollection<float> multiplicativeIncreases)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        ShatterDamage           = shatterDamage;
        AdditiveIncreases       = additiveIncreases;
        MultiplicativeIncreases = multiplicativeIncreases;
    }

    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float ShatterDamage { get; init; }
    public ICollection<float> AdditiveIncreases { get; init; }
    public ICollection<float> MultiplicativeIncreases { get; init; }

    public void Deconstruct(out Team           appliesTo, out SourceTag      source, out float          shatterDamage, out ICollection<float> additiveIncreases, out ICollection<float> multiplicativeIncreases)
    {
        appliesTo               = AppliesTo;
        source                  = Source;
        shatterDamage           = ShatterDamage;
        additiveIncreases       = AdditiveIncreases;
        multiplicativeIncreases = MultiplicativeIncreases;
    }
}