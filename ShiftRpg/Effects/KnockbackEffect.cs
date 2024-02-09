using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Effects;

public record class KnockbackEffect(float Magnitude, float Direction)
{
    public Vector3 KnockbackVector => Vector2Extensions.FromAngleAndLength(Direction, Magnitude).ToVec3();
}