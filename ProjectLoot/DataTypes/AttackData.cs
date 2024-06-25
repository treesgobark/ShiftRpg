using Microsoft.Xna.Framework;

namespace ProjectLoot.DataTypes;

public partial class AttackData
{
    public Vector2 HitboxOffset => new(HitboxOffsetX, HitboxOffsetY);
    public TimeSpan StartupTimeSpan => TimeSpan.FromSeconds(StartupTime);
    public TimeSpan ActiveTimeSpan => TimeSpan.FromSeconds(ActiveTime);
    public TimeSpan RecoveryTimeSpan => TimeSpan.FromSeconds(RecoveryTime);
}