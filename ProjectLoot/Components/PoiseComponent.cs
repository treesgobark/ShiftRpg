using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class PoiseComponent : IPoiseComponent
{
    public float PoiseThreshold { get; set; }
    public float CurrentPoiseDamage { get; set; }

    public bool IsAboveThreshold => CurrentPoiseDamage >= PoiseThreshold;
}