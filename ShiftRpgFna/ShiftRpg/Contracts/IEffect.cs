using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffect
{
    Team AppliesTo { get; }
    SourceTag Source { get; }
    Guid EffectId { get; }
    
    static readonly IReadOnlyList<IEffect> EmptyList = new List<IEffect>();
}