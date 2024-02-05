using System;
using System.Diagnostics;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
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
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Movement     = new Gated2DInput(gamePad.LeftStick, 8);
                Aim          = gamePad.RightStick;
                Attack       = gamePad.GetButton(Xbox360GamePad.Button.RightShoulder);
                Reload       = gamePad.GetButton(Xbox360GamePad.Button.X);
                Dash         = gamePad.GetButton(Xbox360GamePad.Button.LeftShoulder);
                SwitchWeapon = gamePad.GetButton(Xbox360GamePad.Button.Y);
                break;
            case Keyboard keyboard:
                Movement     = keyboard.GetWasdInput();
                Aim          = new VirtualAimer(InputManager.Mouse, player);
                Attack       = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                Reload       = keyboard.GetKey(Keys.R);
                Dash         = keyboard.GetKey(Keys.Space);
                SwitchWeapon = keyboard.GetKey(Keys.Q);
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
    public IPressableInput SwitchWeapon { get; }
}