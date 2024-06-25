using FlatRedBall.Graphics;

namespace ProjectLoot.Contracts;

public interface IWeapon<TInput> : IDestroyable, IUpdateable
{
    IWeaponHolder Holder { get; set; }
    TInput InputDevice { get; set; }

    void Equip();
    void Unequip();
}