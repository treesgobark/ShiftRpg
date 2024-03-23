namespace ShiftRpg.Contracts;

public interface ITakesWeaknessDamage : ITakesDamage
{
    float CurrentWeaknessDamage { get; }
    float MaxWeaknessDamagePercentage { get; }
    
    void TakeWeaknessDamage(float damage);
    void ResetWeaknessDamage();
}