using FlatRedBall.Input;
using ProjectLoot.Contracts;

namespace ProjectLoot.InputDevices;

public class MeleeWeaponInputDevice(IGameplayInputDevice gameplayInputDevice) : IMeleeWeaponInputDevice
{
    public IPressableInput LightAttack { get; set; } = gameplayInputDevice.LightAttack;
    public IPressableInput HeavyAttack { get; set; } = gameplayInputDevice.HeavyAttack;
}

public class ZeroMeleeWeaponInputDevice : IMeleeWeaponInputDevice
{
    public static ZeroMeleeWeaponInputDevice Instance { get; } = new();
    public IPressableInput LightAttack { get; set; } = FalsePressableInput.Instance;
    public IPressableInput HeavyAttack { get; set; } = FalsePressableInput.Instance;
}