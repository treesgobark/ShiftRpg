using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Contracts;

public interface IMeleeWeaponModel : IUpdate
{
    string WeaponName { get; }
    IEffectsComponent HolderEffects { get; }
    IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    bool IsEquipped { get; set; }
}