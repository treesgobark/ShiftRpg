using System;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public record class KnockbackEffect(Team AppliesTo, SourceTag Source, float Magnitude, float Direction) : IEffect
{
    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction, Magnitude).ToVec3();
    public Guid EffectId { get; } = Guid.NewGuid();
}