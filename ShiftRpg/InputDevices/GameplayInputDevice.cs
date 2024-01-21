using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Entities;
using Keyboard = FlatRedBall.Input.Keyboard;
using Mouse = FlatRedBall.Input.Mouse;

namespace ShiftRpg.InputDevices;

public class GameplayInputDevice : IGameplayInputDevice
{
    public GameplayInputDevice(IInputDevice inputDevice, Player player)
    {
        Movement = inputDevice.Default2DInput;
        Attack = inputDevice.DefaultPrimaryActionInput;
        Reload = inputDevice.DefaultSecondaryActionInput;

        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Aim = gamePad.RightStick;
                Attack = Attack.Or(gamePad.GetButton(Xbox360GamePad.Button.RightShoulder));
                Reload = gamePad.GetButton(Xbox360GamePad.Button.X);
                break;
            case Keyboard keyboard:
                Aim = new VirtualAimer(InputManager.Mouse, player);
                Attack = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                Reload = keyboard.GetKey(Keys.R);
                break;
        }
    }
    
    public I2DInput Movement { get; }
    public I2DInput Aim { get; }
    public IPressableInput Attack { get; }
    public IPressableInput Reload { get; }
}