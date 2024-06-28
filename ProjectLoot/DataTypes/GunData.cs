using System.Runtime.Serialization;

namespace ProjectLoot.DataTypes;

public partial class GunData
{
    public TimeSpan TimePerRound => TimeSpan.FromSeconds(SecondsPerRound);
    public TimeSpan ReloadTimeSpan => TimeSpan.FromSeconds(ReloadTime);
}