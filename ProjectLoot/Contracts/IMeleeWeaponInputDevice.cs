using FlatRedBall.Input;

namespace ProjectLoot.Contracts;

public interface IMeleeWeaponInputDevice
{
    IPressableInput Attack { get; set; }
}