using FlatRedBall.Graphics;

namespace ProjectLoot.Contracts;

public interface IWeapon<TInput> : IDestroyable
{
    IWeaponHolder Holder { get; set; }
    
    TInput InputDevice { get; }
    
    void Equip(TInput inputDevice);
    void Unequip();
    void Activity();
}