using System;
using System.Collections.Generic;

namespace ShiftRpg.Contracts;

public interface IMeleeWeapon
{
    Action<IReadOnlyList<object>> ApplyHolderEffects { get; set; }
    
    IReadOnlyList<object> GetTargetHitEffects();
    IReadOnlyList<object> GetHolderHitEffects();
    
    void Equip(IMeleeWeaponInputDevice inputDevice);
    void Unequip();
}