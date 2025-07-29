using System.Collections.Generic;
using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class KnockbackEffect : IEffect
{
    public KnockbackEffect(Team              appliesTo,
                           SourceTag         source,
                           float             value,
                           Rotation          direction,
                           KnockbackBehavior knockbackBehavior,
                           bool              relativeDirection = false)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        Value                   = value;
        Direction               = direction;
        KnockbackBehavior       = knockbackBehavior;
        RelativeDirection       = relativeDirection;
    }

    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction.NormalizedRadians, Value).ToVec3();
    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float Value { get; init; }
    public Rotation Direction { get; init; }
    public KnockbackBehavior KnockbackBehavior { get; init; }
    public bool RelativeDirection { get; init; }
}

public enum KnockbackBehavior
{
    Replacement,
    Additive,
}