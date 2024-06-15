using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Components;

public class HealthComponent : IHealthComponent
{
    public HealthComponent()
    {
        CurrentHealth = MaxHealth;
    }
    
    public required float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public double LastDamageTime { get; set; }
    public IStatModifierCollection<float> DamageModifiers { get; } = new StatModifierCollection<float>();
}