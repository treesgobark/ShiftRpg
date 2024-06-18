using FlatRedBall.Input;

namespace ProjectLoot.InputDevices;

public class TruePressableInput : IPressableInput
{
    public static readonly TruePressableInput Instance = new();
    
    public bool IsDown => true;
    public bool WasJustPressed => true;
    public bool WasJustReleased => true;
}