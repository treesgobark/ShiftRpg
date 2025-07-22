using System.Collections.Generic;
using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class KnockbackEffect : INumericalEffect<float>, IEffect
{
    public KnockbackEffect(Team appliesTo,
        SourceTag source,
        float value,
        Rotation direction,
        KnockbackBehavior knockbackBehavior,
        bool relativeDirection = false) : this(
        appliesTo,
        source,
        value,
        direction,
        knockbackBehavior,
        new List<float>(),
        new List<float>(),
        relativeDirection
        )
    {
        
    }

    public KnockbackEffect(Team              appliesTo,
                           SourceTag         source,
                           float             value,
                           Rotation          direction,
                           KnockbackBehavior knockbackBehavior,
                           List<float>       additiveIncreases,
                           List<float>       multiplicativeIncreases,
                           bool              relativeDirection = false)
    {
        AppliesTo               = appliesTo;
        Source                  = source;
        Value                   = value;
        Direction               = direction;
        KnockbackBehavior       = knockbackBehavior;
        AdditiveIncreases       = additiveIncreases;
        MultiplicativeIncreases = multiplicativeIncreases;
        RelativeDirection       = relativeDirection;
    }

    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction.NormalizedRadians, Value).ToVec3();
    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public float Value { get; init; }
    public Rotation Direction { get; init; }
    public KnockbackBehavior KnockbackBehavior { get; init; }
    public List<float> AdditiveIncreases { get; init; }
    public List<float> MultiplicativeIncreases { get; init; }
    public bool RelativeDirection { get; init; }

    public void Deconstruct(out Team appliesTo, out SourceTag     source, out float         value, out Rotation      direction, out KnockbackBehavior knockbackBehavior, out List<float> additiveIncreases, out List<float> multiplicativeIncreases, out bool     relativeDirection)
    {
        appliesTo               = AppliesTo;
        source                  = Source;
        value                   = Value;
        direction               = Direction;
        knockbackBehavior       = KnockbackBehavior;
        additiveIncreases       = AdditiveIncreases;
        multiplicativeIncreases = MultiplicativeIncreases;
        relativeDirection       = RelativeDirection;
    }
}

public enum KnockbackBehavior
{
    Replacement,
    Additive,
}