using System.Collections.Generic;
using ProjectLoot.Effects;

namespace ProjectLoot.Contracts;

public interface IEffect
{
    Team AppliesTo { get; }
    SourceTag Source { get; }
    Guid EffectId { get; }
    
    static readonly IReadOnlyList<IEffect> EmptyList = new List<IEffect>();
}