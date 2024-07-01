using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace ProjectLoot.DataTypes;

public partial class AttackData
{
    [IgnoreDataMember]
    public Vector2 HitboxOffset => new(HitboxOffsetX, HitboxOffsetY);
    [IgnoreDataMember]
    public TimeSpan StartupTimeSpan => TimeSpan.FromSeconds(StartupTime);
    [IgnoreDataMember]
    public TimeSpan ActiveTimeSpan => TimeSpan.FromSeconds(ActiveTime);
    [IgnoreDataMember]
    public TimeSpan RecoveryTimeSpan => TimeSpan.FromSeconds(RecoveryTime);
}