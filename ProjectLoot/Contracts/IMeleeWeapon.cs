using ProjectLoot.DataTypes;
using ProjectLoot.Entities;

namespace ProjectLoot.Contracts;

public interface IMeleeWeapon : IWeapon<IMeleeWeaponInputDevice>
{
    bool IsActive { get; set; }
    AttackData CurrentAttackData { get; }
    
    MeleeHitbox SpawnHitbox();
}