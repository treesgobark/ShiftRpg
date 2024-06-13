using FlatRedBall.Graphics;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IWeapon<TInput> : IDestroyable
{
    IWeaponHolder Holder { get; set; }
    
    IEffectBundle TargetHitEffects { get; }
    IEffectBundle HolderHitEffects { get; }
    
    SourceTag Source { get; set; }

    TInput InputDevice { get; }
    
    void Equip(TInput inputDevice);
    void Unequip();
}