using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using ProjectLoot.Contracts;
using ProjectLoot.Entities;
using Keyboard = FlatRedBall.Input.Keyboard;
using Mouse = FlatRedBall.Input.Mouse;

namespace ProjectLoot.InputDevices;

public class GameplayInputDevice : IGameplayInputDevice
{
    public GameplayInputDevice(IInputDevice inputDevice, Player player)
    {
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Movement        = new Gated2DInput(gamePad.LeftStick, 8);
                Aim             = gamePad.RightStick;
                Attack          = gamePad.GetButton(Xbox360GamePad.Button.RightShoulder)
                    .Or(gamePad.GetButton(Xbox360GamePad.Button.X));
                Reload          = gamePad.GetButton(Xbox360GamePad.Button.B);
                Dash            = gamePad.GetButton(Xbox360GamePad.Button.LeftShoulder);
                QuickSwapWeapon = gamePad.GetButton(Xbox360GamePad.Button.Y);
                // NextWeapon      = ;
                // PreviousWeapon  = ;
                break;
            case Keyboard keyboard:
                Movement        = keyboard.GetWasdInput();
                Aim             = new VirtualAimer(InputManager.Mouse, player);
                Attack          = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                Reload          = keyboard.GetKey(Keys.R);
                Dash            = keyboard.GetKey(Keys.Space);
                QuickSwapWeapon = keyboard.GetKey(Keys.Q);
                NextWeapon      = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Up);
                PreviousWeapon  = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Down);
                break;
            default:
                throw new ArgumentException("Input device was something other than gamepad or keyboard");
        }
    }
    
    public I2DInput Movement { get; }
    public I2DInput Aim { get; }
    public IPressableInput Attack { get; }
    public IPressableInput Reload { get; }
    public IPressableInput Dash { get; }
    public IPressableInput QuickSwapWeapon { get; }
    public IPressableInput NextWeapon { get; }
    public IPressableInput PreviousWeapon { get; }
    public bool AimInMeleeRange => Aim.Magnitude < 1;
}