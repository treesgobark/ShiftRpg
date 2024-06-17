using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IWeaponsComponent
{
    public IWeaponCache<IGun, IGunInputDevice> GunCache { get; }
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> MeleeWeaponCache { get; }
}