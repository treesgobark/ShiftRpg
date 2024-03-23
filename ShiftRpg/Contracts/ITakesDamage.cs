using System;
using System.Collections.Generic;
using FlatRedBall;

namespace ShiftRpg.Contracts;

public interface ITakesDamage : IEffectReceiver
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    double LastDamageTime { get; set; }
    double InvulnerabilityTimeAfterDamage { get; set; }
    
    float CurrentHealthPercentage { get; }
    double TimeSinceLastDamage { get; }
    bool IsInvulnerable { get; }

    void TakeDamage(float damage);
}