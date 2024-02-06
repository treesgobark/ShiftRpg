using FlatRedBall.Input;

namespace ShiftRpg.InputDevices;

public static class ScrollWheelPressableInput
{
    public static IPressableInput Up = new DelegateBasedPressableInput(
        () => InputManager.Mouse.ScrollWheelChange > 0,
        () => InputManager.Mouse.ScrollWheelChange > 0,
        () => false
    );
    
    public static IPressableInput Down = new DelegateBasedPressableInput(
        () => InputManager.Mouse.ScrollWheelChange < 0,
        () => InputManager.Mouse.ScrollWheelChange < 0,
        () => false
    );
}