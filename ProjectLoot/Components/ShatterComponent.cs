using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class ShatterComponent : IShatterComponent
{
    public float CurrentShatterDamage { get; set; }
    public float MaxShatterDamagePercentage { get; set; } = 20;
}
