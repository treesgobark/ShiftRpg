using System.Collections.Generic;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class HealthReductionEffect : IEffect
{
    public HealthReductionEffect(Team        appliesTo,
                        SourceTag   source,
                        float       value)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        Value                   = value;
    }

    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float Value { get; init; }
}