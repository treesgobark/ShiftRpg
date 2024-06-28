using ProjectLoot.DataTypes;

namespace ProjectLoot.Contracts;

public interface IGunModel
{
    GunData GunData { get; }
    int CurrentRoundsInMagazine { get; set; }
}

public enum FiringType
{
    SingleShot,
    Automatic,
}