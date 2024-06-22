namespace ProjectLoot.Components.Interfaces;

public interface IWeaknessComponent
{
    float CurrentWeaknessPercentage { get; set; }

    float DepletionRatePerSecond { get; set; }
}