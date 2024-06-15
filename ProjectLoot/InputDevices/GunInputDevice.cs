using FlatRedBall.Input;
using ProjectLoot.Contracts;

namespace ProjectLoot.InputDevices;

public class GunInputDevice : IGunInputDevice
{
    public IGameplayInputDevice GameplayInputDevice { get; }

    public GunInputDevice(IGameplayInputDevice gameplayInputDevice)
    {
        GameplayInputDevice = gameplayInputDevice;
    }

    public IPressableInput Fire => GameplayInputDevice.Attack;
    public IPressableInput Reload => GameplayInputDevice.Reload;
}

public class ZeroGunInputDevice : IGunInputDevice
{
    public static ZeroGunInputDevice Instance { get; } = new();

    public IPressableInput Fire { get; } = FalsePressableInput.Instance;
    public IPressableInput Reload { get; } = FalsePressableInput.Instance;
}