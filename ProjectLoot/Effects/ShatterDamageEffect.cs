using System.Collections.Generic;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record ShatterDamageEffect(
    Team AppliesTo,
    SourceTag Source,
    float ShatterDamage,
    ICollection<float> AdditiveIncreases,
    ICollection<float> MultiplicativeIncreases)
{
    public ShatterDamageEffect(Team AppliesTo, SourceTag Source, float ShatterDamage)
        : this(AppliesTo, Source, ShatterDamage, new List<float>(), new List<float>())
    {
        
    }
}