namespace ProjectLoot.Components.Interfaces;

public interface IShatterComponent
{
    float CurrentShatterDamage { get; }
    float MaxShatterDamagePercentage { get; set; }
    float CurrentShatterPercentage { get; }
    
    void SetShatterDamage(float shatterDamage, IHealthComponent health);
}