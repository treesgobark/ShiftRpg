using System;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffect
{
    Team AppliesTo { get; }
    Guid EffectId { get; }
}