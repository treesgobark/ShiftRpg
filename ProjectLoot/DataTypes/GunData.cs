using System.Runtime.Serialization;
using ProjectLoot.Components;

namespace ProjectLoot.DataTypes;

public partial class GunData
{
    [IgnoreDataMember]
    public TimeSpan TimePerRound => TimeSpan.FromSeconds(SecondsPerRound);
    [IgnoreDataMember]
    public TimeSpan ReloadTimeSpan => TimeSpan.FromSeconds(ReloadTime);
    [IgnoreDataMember]
    public GunClass GunClass => GunName switch
    {
        Pistol => GunClass.Handgun,
        Rifle => GunClass.Rifle,
        Shotgun => GunClass.Shotgun,
        _ => throw new InvalidOperationException($"Gun name {GunName} is not a valid GunClass"),
    };
}

public enum GunClass
{
    Handgun,
    Rifle,
    Shotgun,
}