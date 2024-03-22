namespace ShiftRpg.Contracts;

public interface ITakesWeaknessDamage : ITakesDamage
{
    int CurrentWeaknessDamage { get; }
    float MaxWeaknessDamagePercentage { get; }
    
    void TakeWeaknessDamage(int damage);
    void ResetWeaknessDamage();
}