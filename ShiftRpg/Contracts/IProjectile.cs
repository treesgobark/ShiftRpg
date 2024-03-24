using System;
using System.Collections.Generic;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Contracts;

public interface IProjectile : IDestroyable, IPositionable
{
    Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
    IReadOnlyList<IEffect> TargetHitEffects { get; set; }
    IReadOnlyList<IEffect> HolderHitEffects { get; set; }
    bool IsActive { get; }

    void InitializeProjectile(float projectileRadius, Vector3 projectileSpeed,
        Action<IReadOnlyList<IEffect>> applyHolderEffects, IReadOnlyList<IEffect> targetHitEffects,
        IReadOnlyList<IEffect> holderHitEffects);
}