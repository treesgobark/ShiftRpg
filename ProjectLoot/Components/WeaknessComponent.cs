using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class WeaknessComponent : IWeaknessComponent
{
    public float CurrentWeaknessAmount { get; set; }
    public float MaxWeaknessDamagePercentage { get; set; }
}
