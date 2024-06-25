using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public class MeleeWeaponComponent : IMeleeWeaponComponent
{
    public MeleeWeaponComponent(IMeleeWeaponInputDevice gunInputDevice)
    {
        Cache = new WeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice>(gunInputDevice);
    }
    
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> Cache { get; }
}