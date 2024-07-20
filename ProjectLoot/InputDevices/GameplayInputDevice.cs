using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Input;
using FlatRedBall.Math;
using Microsoft.Xna.Framework.Input;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using Keyboard = FlatRedBall.Input.Keyboard;
using Mouse = FlatRedBall.Input.Mouse;

namespace ProjectLoot.InputDevices;

public class GameplayInputDevice : IGameplayInputDevice
{
    private readonly I2DInput _aim;
    private bool _inputEnabled = true;

    public GameplayInputDevice(IInputDevice inputDevice, IPositionable position, float meleeAimThreshold)
    {
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Movement = new Gated2DInput(gamePad.LeftStick, 8);
                _aim = gamePad.RightStick;
                Attack = gamePad.GetButton(Xbox360GamePad.Button.RightShoulder)
                    .Or(gamePad.GetButton(Xbox360GamePad.Button.X));
                Reload          = gamePad.GetButton(Xbox360GamePad.Button.B);
                Dash            = gamePad.GetButton(Xbox360GamePad.Button.LeftShoulder);
                Guard           = gamePad.GetButton(Xbox360GamePad.Button.LeftTrigger);
                QuickSwapWeapon = gamePad.GetButton(Xbox360GamePad.Button.Y);
                NextWeapon      = gamePad.GetButton(Xbox360GamePad.Button.RightTrigger);
                PreviousWeapon  = FalsePressableInput.Instance;
                Interact        = gamePad.GetButton(Xbox360GamePad.Button.A);
                // NextWeapon      = ;
                // PreviousWeapon  = ;
                break;
            case Keyboard keyboard:
                Movement = keyboard.GetWasdInput();
                _aim = new VirtualAimer(InputManager.Mouse, position, meleeAimThreshold);
                Attack = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                Reload = keyboard.GetKey(Keys.R);
                Dash = InputManager.Mouse.GetButton(Mouse.MouseButtons.RightButton);
                Guard = keyboard.GetKey(Keys.LeftShift);
                QuickSwapWeapon = keyboard.GetKey(Keys.Q);
                NextWeapon = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Up);
                PreviousWeapon = InputManager.Mouse.GetPressableScrollWheel(MouseExtensions.WheelDirection.Down);
                Interact = keyboard.GetKey(Keys.E);
                break;
            default:
                throw new ArgumentException("Input device was something other than gamepad or keyboard");
        }

        GunInputDevice = new GunInputDevice(this);
        MeleeWeaponInputDevice = new MeleeWeaponInputDevice(this);
    }

    private Constant2DInput ConstantAim { get; set; }

    public I2DInput Movement { get; }

    public I2DInput Aim
    {
        get
        {
            if (!InputEnabled) return ConstantAim;
            return _aim;
        }
    }

    public IPressableInput Attack { get; private set; }
    public IPressableInput Reload { get; private set; }
    public IPressableInput Dash { get; private set; }
    public IPressableInput Guard { get; private set; }
    public IPressableInput QuickSwapWeapon { get; }
    public IPressableInput NextWeapon { get; }
    public IPressableInput PreviousWeapon { get; }
    public IPressableInput Interact { get; }
    public IGunInputDevice GunInputDevice { get; private set; }
    public IMeleeWeaponInputDevice MeleeWeaponInputDevice { get; private set; }

    public void BufferAttackPress()
    {
    }

    public void ApplyHitstopGuardClauses(IHitstopComponent hitstopComponent)
    {
        Attack = Attack.And(new DelegateBasedPressableInput(() => !hitstopComponent.IsStopped,
                                                            () => !hitstopComponent.IsStopped,
                                                            () => !hitstopComponent.IsStopped));
        
        Reload = Reload.And(new DelegateBasedPressableInput(() => !hitstopComponent.IsStopped,
                                                            () => !hitstopComponent.IsStopped,
                                                            () => !hitstopComponent.IsStopped));
            
        Dash = Dash.And(new DelegateBasedPressableInput(() => !hitstopComponent.IsStopped,
                                                        () => !hitstopComponent.IsStopped,
                                                        () => !hitstopComponent.IsStopped));
            
        Guard = Guard.And(new DelegateBasedPressableInput(() => !hitstopComponent.IsStopped,
                                                          () => !hitstopComponent.IsStopped,
                                                          () => !hitstopComponent.IsStopped));

        GunInputDevice = new GunInputDevice(this);
        MeleeWeaponInputDevice = new MeleeWeaponInputDevice(this);
    }

    public bool AimInMeleeRange => Aim.Magnitude < 1;


    public bool InputEnabled
    {
        get => _inputEnabled;
        set
        {
            if ((_inputEnabled, value) is (true, false)) ConstantAim = new Constant2DInput(Aim);
            _inputEnabled = value;
        }
    }
}