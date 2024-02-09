using System;
using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IMeleeWeapon
{
    Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
    IReadOnlyList<IEffect> TargetHitEffects { get; set; }
    IReadOnlyList<IEffect> HolderHitEffects { get; set; }
    Team Team { get; set; }
    
    void Equip(IMeleeWeaponInputDevice inputDevice);
    void Unequip();
}