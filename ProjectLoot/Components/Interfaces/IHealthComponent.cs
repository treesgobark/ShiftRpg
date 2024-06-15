using FlatRedBall;
using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IHealthComponent
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }
    double LastDamageTime { get; set; }
    
    float CurrentHealthPercentage => 100f * CurrentHealth / MaxHealth;
    double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(LastDamageTime);
    // bool IsInvulnerable => TimeSinceLastDamage < InvulnerabilityTimeAfterDamage;
    bool IsInvulnerable => false;
    
    IStatModifierCollection<float> DamageModifiers { get; }
}