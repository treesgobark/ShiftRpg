using FlatRedBall.Input;

namespace ProjectLoot.Contracts;

public interface IGameplayInputDevice
{
    I2DInput Movement { get; }
    I2DInput Aim { get; }
    IPressableInput Attack { get; }
    IPressableInput Reload { get; }
    IPressableInput Dash { get; }
    IPressableInput Guard { get; }
    IPressableInput QuickSwapWeapon { get; }
    IPressableInput NextWeapon { get; }
    IPressableInput PreviousWeapon { get; }
    
    bool AimInMeleeRange { get; }
    bool InputEnabled { get; set; }
}