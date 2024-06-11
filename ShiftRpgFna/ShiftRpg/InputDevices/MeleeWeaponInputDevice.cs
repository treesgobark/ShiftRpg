using FlatRedBall.Input;
using ShiftRpg.Contracts;

namespace ShiftRpg.InputDevices;

public class MeleeWeaponInputDevice(IGameplayInputDevice gameplayInputDevice) : IMeleeWeaponInputDevice
{
    public IPressableInput Attack { get; set; } = gameplayInputDevice.Attack;
}

public class ZeroMeleeWeaponInputDevice : IMeleeWeaponInputDevice
{
    public static ZeroMeleeWeaponInputDevice Instance { get; } = new ZeroMeleeWeaponInputDevice();
    public IPressableInput Attack { get; set; } = new FalsePressableInput();
}