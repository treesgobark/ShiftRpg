using System.Collections.Generic;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.FlatRedBall.Extensions;
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
}

public enum KnockbackBehavior
{
    Replacement,
    Additive,
}