using ProjectLoot.DataTypes;

namespace ProjectLoot.Contracts;

public interface IGunModel : IUpdateable
{
    GunData GunData { get; }
    
    bool IsEquipped { get; set; }
}
