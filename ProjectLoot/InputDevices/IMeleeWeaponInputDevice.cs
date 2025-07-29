using FlatRedBall.Input;

namespace ProjectLoot.Contracts;

public interface IMeleeWeaponInputDevice
{
    IPressableInput LightAttack { get; set; }
    IPressableInput HeavyAttack { get; set; }
    IPressableInput Modifier { get; set; }
}