using FlatRedBall.Forms.MVVM;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.GumRuntimes;

namespace ProjectLoot.Components;

public class HealthComponent : ViewModel, IHealthComponent
{
    public HealthComponent(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }
    
    public HealthComponent(float maxHealth, HealthBarRuntime healthBarRuntime) : this(maxHealth)
    {
        healthBarRuntime.MainBar.BindingContext = this;
        healthBarRuntime.MainBar.SetBinding(nameof(HealthBarRuntime.MainBar.ProgressPercentage), nameof(HealthPercentage));
    }

    public float MaxHealth
    {
        get => Get<float>();
        set
        {
            float newValue = MathHelper.Clamp(value, 1, float.MaxValue);
            CurrentHealth = newValue;
            Set(newValue);
        }
    }

    public float CurrentHealth
    {
        get => Get<float>();
        set
        {
            float newValue = MathHelper.Clamp(value, 0, MaxHealth);
            Set(newValue);
        }
    }

    [DependsOn(nameof(CurrentHealth))]
    [DependsOn(nameof(MaxHealth))]
    public float HealthPercentage => MaxHealth > 0 
        ? 100 * CurrentHealth / MaxHealth
        : 100;

    public TimeSpan LastDamageTime { get; set; }
    public IStatModifierCollection<float> DamageModifiers { get; } = new StatModifierCollection<float>();
}