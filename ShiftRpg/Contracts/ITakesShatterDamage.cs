namespace ShiftRpg.Contracts;

public interface ITakesShatterDamage : ITakesDamage
{
    float CurrentShatterDamage { get; }
    float MaxShatterDamagePercentage { get; }
    
    void TakeShatterDamage(float damage);
    void ResetShatterDamage();
}