using ProjectLoot.DataTypes;

namespace ProjectLoot.Contracts;

public interface IMeleeWeaponModel : IUpdateable
{
    MeleeWeaponData MeleeWeaponData { get; }
    
    bool IsEquipped { get; set; }
}