using FlatRedBall.Forms.MVVM;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.GumRuntimes;

namespace ProjectLoot.Components;

public class ShatterComponent : ViewModel, IShatterComponent
{
    public ShatterComponent()
    {
        MaxShatterDamagePercentage = .2f;
    }
    
    public ShatterComponent(HealthBarRuntime healthBarRuntime) : this()
    {
        healthBarRuntime.MainBar.SubProgress.BindingContext = this;
        healthBarRuntime.MainBar.SubProgress.SetBinding(nameof(HealthBarRuntime.MainBar.SubProgress.Width), nameof(CurrentShatterPercentage));
    }

    public float CurrentShatterDamage
    {
        get => Get<float>();
        set => Set(value);
    }

    public float MaxShatterDamagePercentage
    {
        get => Get<float>();
        set => Set(value);
    }
    
    public float CurrentShatterPercentage
    {
        get => Get<float>();
        set => Set(value);
    }

    public void SetShatterDamage(float shatterDamage, IHealthComponent health)
    {
        shatterDamage = Math.Clamp(shatterDamage, 0, MaxShatterDamagePercentage * health.MaxHealth);
        CurrentShatterDamage = shatterDamage;
        if (CurrentShatterDamage > health.CurrentHealth || health.CurrentHealth <= 0)
        {
            CurrentShatterPercentage = 100f;
        }
        else
        {
            CurrentShatterPercentage = CurrentShatterDamage / health.CurrentHealth * 100f;
        }
    }
}
