using FlatRedBall.Input;

namespace ShiftRpg.Contracts;

public interface IMeleeWeaponInputDevice
{
    IPressableInput Attack { get; set; }
}