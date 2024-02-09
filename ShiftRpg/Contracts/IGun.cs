using System;
using System.Collections.Generic;
using ShiftRpg.Effects;
using ShiftRpg.Entities;

namespace ShiftRpg.Contracts;

public interface IGun
{
    Action<IReadOnlyList<IEffect>> ApplyHolderEffects { set; }
    IReadOnlyList<IEffect> TargetHitEffects { get; }
    IReadOnlyList<IEffect> HolderHitEffects { get; }
    Team Team { get; set; }
    
    void Equip(IGunInputDevice inputDevice);
    void Unequip();

    Projectile SpawnProjectile();
}