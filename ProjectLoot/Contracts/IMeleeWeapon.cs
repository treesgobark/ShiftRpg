using ProjectLoot.DataTypes;
using ProjectLoot.Entities;

namespace ProjectLoot.Contracts;

public interface IMeleeWeapon : IWeapon<IMeleeWeaponInputDevice>
{
    AttackData CurrentAttackData { get; }
    
    MeleeHitbox SpawnHitbox();
}