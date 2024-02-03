using Microsoft.Xna.Framework;

namespace ShiftRpg.Contracts;

public interface IMeleeWeapon : IWeapon
{
    void BeginAttack();
    void EndAttack();
}