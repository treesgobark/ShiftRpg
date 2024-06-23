using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Input;
using FlatRedBall.Math;
using Microsoft.Xna.Framework.Input;
using ProjectLoot.Contracts;
using ProjectLoot.Entities;
using Keyboard = FlatRedBall.Input.Keyboard;
using Mouse = FlatRedBall.Input.Mouse;

namespace ProjectLoot.InputDevices;

public class GameplayInputDevice : IGameplayInputDevice
{
    private bool _inputEnabled = true;
    private readonly I2DInput _aim;

    public GameplayInputDevice(IInputDevice inputDevice, IPositionable position, float meleeAimThreshold)
    {
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Movement        = new Gated2DInput(gamePad.LeftStick, 8);
                _aim            = gamePad.RightStick;
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
                _aim            = new VirtualAimer(InputManager.Mouse, position, meleeAimThreshold);
                Attack          = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                Reload          = keyboard.GetKey(Keys.R);
                Dash            = InputManager.Mouse.GetButton(Mouse.MouseButtons.RightButton);
                QuickSwapWeapon = keyboard.GetKey(Keys.Q);
                NextWeapon      = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Up);
                PreviousWeapon  = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Down);
                break;
            default:
                throw new ArgumentException("Input device was something other than gamepad or keyboard");
        }
    }
    
    public I2DInput Movement { get; }

    public I2DInput Aim
    {
        get
        {
            if (!InputEnabled)
            {
                return ConstantAim;
            }
            return _aim;
        }
    }

    private Constant2DInput ConstantAim { get; set; }
    public IPressableInput Attack { get; }
    public IPressableInput Reload { get; }
    public IPressableInput Dash { get; }
    public IPressableInput QuickSwapWeapon { get; }
    public IPressableInput NextWeapon { get; }
    public IPressableInput PreviousWeapon { get; }
    public bool AimInMeleeRange => Aim.Magnitude < 1;
    

    public bool InputEnabled
    {
        get => _inputEnabled;
        set
        {
            if ((_inputEnabled, value) is (true, false))
            {
                ConstantAim = new Constant2DInput(Aim);
            }
            _inputEnabled = value;
        }
    }
}