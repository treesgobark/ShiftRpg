namespace ProjectLoot.Contracts;

public interface IMeleeWeapon : IWeapon<IMeleeWeaponInputDevice>
{
    bool IsActive { get; set; }
}