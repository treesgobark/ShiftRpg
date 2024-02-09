using System;
using System.Collections.Generic;
using FlatRedBall.Graphics;
using FlatRedBall.Math;

namespace ShiftRpg.Contracts;

public interface IProjectile : IDestroyable
{
    Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
    IReadOnlyList<IEffect> TargetHitEffects { get; set; }
    IReadOnlyList<IEffect> HolderHitEffects { get; set; }
    bool IsActive { get; }
}