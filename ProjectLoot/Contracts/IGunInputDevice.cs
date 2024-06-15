using FlatRedBall.Input;

namespace ProjectLoot.Contracts;

public interface IGunInputDevice
{
    IPressableInput Fire { get; }
    IPressableInput Reload { get; }
}