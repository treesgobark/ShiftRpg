using FlatRedBall;
using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IHealthComponent
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }
    double LastDamageTime { get; set; }
    
    IStatModifierCollection<float> DamageModifiers { get; }
}