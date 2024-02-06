using FlatRedBall.Input;

namespace ShiftRpg.Contracts;

public interface IGunInputDevice
{
    IPressableInput Fire { get; }
    IPressableInput Reload { get; }
}