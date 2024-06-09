using ShiftRpg.Effects.Handlers;

namespace ShiftRpg.Contracts;

public interface ITakesWeaknessDamage : ITakesDamage
{
    float CurrentWeaknessAmount { get; set; }
    float MaxWeaknessAmount { get; }
}