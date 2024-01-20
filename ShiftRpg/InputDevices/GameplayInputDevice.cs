using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Entities;

namespace ShiftRpg.InputDevices;

public class GameplayInputDevice : IGameplayInputDevice
{
    public GameplayInputDevice(IInputDevice inputDevice, Player player)
    {
        Movement = inputDevice.Default2DInput;
        Attack = inputDevice.DefaultPrimaryActionInput;
        Reload = inputDevice.DefaultSecondaryActionInput;

        if (inputDevice is Xbox360GamePad gamePad)
        {
            Aim = gamePad.RightStick;
            Attack = Attack.Or(gamePad.GetButton(Xbox360GamePad.Button.RightShoulder));
        }

        if (inputDevice is Keyboard keyboard)
        {
            Aim = new VirtualAimer(InputManager.Mouse, player);
            Attack = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
        }
    }
    
    public I2DInput Movement { get; }
    public I2DInput Aim { get; }
    public IPressableInput Attack { get; }
    public IPressableInput Reload { get; }
}