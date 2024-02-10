using System;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffect
{
    Team AppliesTo { get; }
    SourceTag Source { get; }
    Guid EffectId { get; }
}