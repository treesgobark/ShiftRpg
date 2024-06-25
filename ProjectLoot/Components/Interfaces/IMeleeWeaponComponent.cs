using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IMeleeWeaponComponent
{
    IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> Cache { get; }
}
