using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects;

public record class KnockbackEffect(Team AppliesTo, SourceTag Source, float Magnitude,
    Rotation Direction, KnockbackBehavior KnockbackBehavior, bool RelativeDirection = false) : IEffect
{
    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction.NormalizedRadians, Magnitude).ToVec3();
    public Guid EffectId { get; } = Guid.NewGuid();
}

public enum KnockbackBehavior
{
    Replacement,
    Additive,
}