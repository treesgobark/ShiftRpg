using System;
using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IWeapon<TInput>
{
    Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
    Action<IReadOnlyList<IEffect>> ModifyTargetEffects { get; set; }
    
    IReadOnlyList<IEffect> TargetHitEffects { get; }
    IReadOnlyList<IEffect> HolderHitEffects { get; }
    
    Team Team { get; set; }
    SourceTag Source { get; set; }

    TInput InputDevice { get; }
    
    void Equip(TInput inputDevice);
    void Unequip();
}