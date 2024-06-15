using FlatRedBall.Graphics;

namespace ProjectLoot.Contracts;

public interface IWeaponCache<TWeapon, TInput> : IDestroyable
    where TWeapon : IWeapon<TInput>
{
    public TWeapon CurrentWeapon { get; }
    public int Count { get; }
    public int MaxWeapons { get; }
    public bool IsActive { get; set; }
    public TInput InputDevice { get; set; }
    
    public TWeapon CycleToNextWeapon();
    public TWeapon CycleToPreviousWeapon();
    public TWeapon Add(TWeapon weapon);
    void RemoveWeapon(int index);
    void ReplaceWeapon(int index, TWeapon weapon);
    TWeapon GetWeaponAt(int index);
}