using FlatRedBall.Forms.MVVM;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.GumRuntimes;

namespace ProjectLoot.Components;

public class WeaknessComponent : ViewModel, IWeaknessComponent
{
    public WeaknessComponent() { }

    public WeaknessComponent(HealthBarRuntime healthBarRuntime)
    {
        healthBarRuntime.WeaknessBar.BindingContext = this;
        healthBarRuntime.WeaknessBar.SetBinding(nameof(HealthBarRuntime.WeaknessBar.ProgressPercentage), nameof(CurrentWeaknessPercentage));
    }
    
    public float CurrentWeaknessPercentage
    {
        get => Get<float>();
        set
        {
            float newValue = MathHelper.Clamp(value, 0, 100);
            Set(newValue);
        }
    }

    public float DepletionRatePerSecond { get; set; }
    public float DamageConversionRate { get; set; } = .015f;
}
