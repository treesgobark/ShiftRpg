using Microsoft.Xna.Framework;

namespace ProjectLoot.DataTypes;

public partial class AttackData
{
    public Vector2 HitboxOffset => new(HitboxOffsetX, HitboxOffsetY);
}