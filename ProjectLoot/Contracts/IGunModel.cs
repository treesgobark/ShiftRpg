using ProjectLoot.DataTypes;

namespace ProjectLoot.Contracts;

public interface IGunModel
{
    GunData GunData { get; }
    int CurrentRoundsInMagazine { get; set; }
    bool IsFull { get; }
    bool IsEmpty { get; }
}
