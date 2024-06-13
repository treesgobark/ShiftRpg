namespace ShiftRpg.Contracts;

public interface ITakesShatterDamage : ITakesDamage
{
    float CurrentShatterDamage { get; set; }
    float MaxShatterDamagePercentage { get; }
}