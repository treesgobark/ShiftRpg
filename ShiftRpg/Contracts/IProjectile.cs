using System;
using System.Collections.Generic;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Contracts;

public interface IProjectile : IDestroyable, IPositionable
{
    IEffectBundle TargetHitEffects { get; set; }
    IEffectBundle HolderHitEffects { get; set; }
    bool IsActive { get; }

    void InitializeProjectile(float projectileRadius, Vector3 projectileSpeed, IEffectBundle targetHitEffects,
        IEffectBundle holderHitEffects);
}