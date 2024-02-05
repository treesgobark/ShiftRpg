using FlatRedBall.Input;

namespace ShiftRpg.Contracts;

public interface IGameplayInputDevice
{
    I2DInput Movement { get; }
    I2DInput Aim { get; }
    IPressableInput Attack { get; }
    IPressableInput Reload { get; }
    IPressableInput Dash { get; }
    IPressableInput SwitchWeapon { get; }
}