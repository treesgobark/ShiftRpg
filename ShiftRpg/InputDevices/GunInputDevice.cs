using FlatRedBall.Input;
using ShiftRpg.Contracts;

namespace ShiftRpg.InputDevices;

public class GunInputDevice(IGameplayInputDevice gameplayInputDevice) : IGunInputDevice
{
    public IPressableInput Fire { get; } = gameplayInputDevice.Attack;
    public IPressableInput Reload { get; } = gameplayInputDevice.Reload;
}

public class ZeroGunInputDevice : IGunInputDevice
{
    public static ZeroGunInputDevice Instance { get; } = new ZeroGunInputDevice();

    public IPressableInput Fire { get; } = new FalsePressableInput();
    public IPressableInput Reload { get; } = new FalsePressableInput();
}