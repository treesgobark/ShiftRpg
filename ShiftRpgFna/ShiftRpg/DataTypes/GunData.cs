using System.Runtime.Serialization;

namespace ShiftRpg.DataTypes;

public partial class GunData
{
    [IgnoreDataMember]
    public double SecondsPerRound => 1 / RoundsPerSecond;
}