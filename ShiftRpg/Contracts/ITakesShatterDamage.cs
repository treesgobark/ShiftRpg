namespace ShiftRpg.Contracts;

public interface ITakesShatterDamage : ITakesDamage
{
    int CurrentShatterDamage { get; }
    float MaxShatterDamagePercentage { get; }
    
    void TakeShatterDamage(int damage);
    void ResetShatterDamage();
}