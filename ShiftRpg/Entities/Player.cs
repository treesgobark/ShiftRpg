using System;
using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall.Entities;
using FlatRedBall.Graphics;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Player;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities;

public partial class Player : IHasControllers<Player, PlayerController>
{
    public IGun Gun { get; set; }
    public IMeleeWeapon MeleeWeapon { get; set; }
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public IGunInputDevice GunInputDevice { get; set; }
    public ControllerCollection<Player, PlayerController> Controllers { get; protected set; }
    public bool AimInMeleeRange => GameplayInputDevice.Aim.Magnitude < 1;
    public float LastMeleeRotation { get; set; }

    private void CustomInitialize()
    {
        InitializeGun();
        InitializeMeleeWeapon();
        InitializeControllers();
        ReactToDamageReceived += OnReactToDamageReceived;
        var hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        InvulnerabilityTimeAfterDamage = 0.5;
        // InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
    }

    private void InitializeControllers()
    {
        Controllers = new ControllerCollection<Player, PlayerController>();
        Controllers.Add(new Idle(this));
        Controllers.Add(new MeleeMode(this));
        Controllers.Add(new GunMode(this));
        Controllers.InitializeStartingController<MeleeMode>();
    }

    private void InitializeGun()
    {
        var gun = DefaultGunFactory.CreateNew();
            
        gun.RelativeX = 10;
        gun.AttachTo(this);
        gun.ParentRotationChangesRotation = true;
        gun.ApplyImpulse = ApplyImpulse;
            
        Gun = gun;
    }

    private void InitializeMeleeWeapon()
    {
        var melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Owner                         = this;
        // melee.RelativeX                     = 24;
        melee.ParentRotationChangesRotation = true;
        melee.IsDamageDealingEnabled        = false;

        MeleeWeapon = melee;
    }

    partial void CustomInitializeTopDownInput()
    {
        GameplayInputDevice = new GameplayInputDevice(InputDevice, this);
        GunInputDevice      = new GunInputDevice(GameplayInputDevice);
    }

    private void CustomActivity()
    {
        Controllers.DoCurrentControllerActivity();
    }

    private void CustomDestroy()
    {
        var gun = (IDestroyable)Gun;
        gun.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName) { }

    private void OnReactToDamageReceived(decimal damage, IDamageArea area)
    {
        if (area is Enemy enemy)
        {
            Velocity += enemy.Velocity.NormalizedOrZero() * enemy.KnockbackVelocity;
        }

        HealthBar.ProgressPercentage = (float)(100 * CurrentHealth / MaxHealth);
    }

    private void ApplyImpulse(object obj)
    {
        throw new NotImplementedException();
    }
}