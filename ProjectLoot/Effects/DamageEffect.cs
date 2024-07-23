using System.Collections.Generic;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class DamageEffect : INumericalEffect<float>, IEffect
{
    public DamageEffect(Team appliesTo, SourceTag source, float value)
        : this(appliesTo, source, value, [], []) { }

    public DamageEffect(Team        appliesTo,
                        SourceTag   source,
                        float       value,
                        List<float> additiveIncreases,
                        List<float> multiplicativeIncreases)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        Value                   = value;
        AdditiveIncreases       = additiveIncreases;
        MultiplicativeIncreases = multiplicativeIncreases;
    }

    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float Value { get; init; }
    public List<float> AdditiveIncreases { get; init; }
    public List<float> MultiplicativeIncreases { get; init; }

    public void Deconstruct(out Team    appliesTo, out SourceTag source, out float   value, out List<float> additiveIncreases, out List<float> multiplicativeIncreases)
    {
        appliesTo               = AppliesTo;
        source                  = Source;
        value                   = Value;
        additiveIncreases       = AdditiveIncreases;
        multiplicativeIncreases = MultiplicativeIncreases;
    }
}