namespace ProjectLoot.Components.Interfaces;

public interface IWeaknessComponent
{
    float CurrentWeaknessAmount { get; set; }
    float MaxWeaknessDamagePercentage { get; set; }
}