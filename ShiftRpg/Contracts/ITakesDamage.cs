using FlatRedBall;
using FlatRedBall.Math;

namespace ShiftRpg.Contracts;

public interface ITakesDamage : IEffectReceiver, IPositionable
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; }
    double LastDamageTime { get; set; }
    double InvulnerabilityTimeAfterDamage { get; }
    
    float CurrentHealthPercentage => 100f * CurrentHealth / MaxHealth;
    double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(LastDamageTime);
    bool IsInvulnerable => TimeSinceLastDamage < InvulnerabilityTimeAfterDamage;
}