using ProjectLoot.Components.Interfaces;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Contracts;

public interface IMeleeWeaponModel : IUpdateable
{
    MeleeWeaponData MeleeWeaponData { get; }
    IEffectsComponent HolderEffects { get; }
    IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    bool IsEquipped { get; set; }

    void EvaluateExitConditions();
}