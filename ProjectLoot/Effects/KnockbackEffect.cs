using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record KnockbackEffect(
    Team AppliesTo,
    SourceTag Source,
    float Value,
    Rotation Direction,
    KnockbackBehavior KnockbackBehavior,
    List<float> AdditiveIncreases,
    List<float> MultiplicativeIncreases,
    bool RelativeDirection = false) : INumericalEffect<float>
{
    public KnockbackEffect(Team AppliesTo,
        SourceTag Source,
        float Value,
        Rotation Direction,
        KnockbackBehavior KnockbackBehavior,
        bool RelativeDirection = false) : this(
        AppliesTo,
        Source,
        Value,
        Direction,
        KnockbackBehavior,
        new List<float>(),
        new List<float>(),
        RelativeDirection
        )
    {
        
    }
    
    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction.NormalizedRadians, Value).ToVec3();
    public Guid EffectId { get; } = Guid.NewGuid();
}

public enum KnockbackBehavior
{
    Replacement,
    Additive,
}