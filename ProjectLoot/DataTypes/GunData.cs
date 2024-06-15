using System.Runtime.Serialization;

namespace ProjectLoot.DataTypes;

public partial class GunData
{
    [IgnoreDataMember]
    public double SecondsPerRound => 1 / RoundsPerSecond;
}