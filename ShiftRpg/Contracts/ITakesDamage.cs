using System;
using System.Collections.Generic;
using FlatRedBall;

namespace ShiftRpg.Contracts;

public interface ITakesDamage : IEffectReceiver
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    double LastDamageTime { get; set; }
    double InvulnerabilityTimeAfterDamage { get; set; }
    
    float CurrentHealthPercentage { get; }
    double TimeSinceLastDamage { get; }
    bool IsInvulnerable { get; }

    void TakeDamage(int damage);
}