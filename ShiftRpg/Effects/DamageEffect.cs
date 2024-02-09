using System;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record class DamageEffect(Team AppliesTo, Guid EffectId, int Damage) : IEffect;