using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Debugging;
using FlatRedBall.Entities;
using FlatRedBall.Graphics;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities;

public partial class Player
{
    private IGun Gun { get; set; }
    private IMeleeWeapon MeleeWeapon { get; set; }
    private IGameplayInputDevice GameplayInputDevice { get; set; }
    private bool _meleeLastFrame = true;
    private bool AimInMeleeRange => GameplayInputDevice.Aim.Magnitude < 1;
    private float _lastMeleeRotation = 0;
        
    private void CustomInitialize()
    {
        InitializeGun();
        InitializeMeleeWeapon();
        ReactToDamageReceived += OnReactToDamageReceived;
        var hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        AimThresholdCircle.Radius               = MeleeAimThreshold;
        Gun.Unequip();
        MeleeWeapon.Equip();
        InvulnerabilityTimeAfterDamage = 0.5;
        InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
    }

    private void InitializeGun()
    {
        var gun = DefaultGunFactory.CreateNew();
            
        gun.RelativeX = 10;
        gun.AttachTo(this);
        gun.ParentRotationChangesRotation = true;
            
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
    }

    private void CustomActivity()
    {
        // Debugger.Write($"Is player Invulnerable: {IsInvulnerable}");
        SetRotation();
        HandleInput();
    }

    private void SetRotation()
    {
        if (!InputEnabled)
        {
            return;
        }
        
        float? angle = AimInMeleeRange
            ? GameplayInputDevice.Movement.GetAngle()
            : GameplayInputDevice.Aim.GetAngle();
            
        if (angle is null)
        {
            RotationZ = _lastMeleeRotation;
        }
        else
        {
            RotationZ = angle.Value;
            if (AimInMeleeRange)
            {
                _lastMeleeRotation = RotationZ;
            }
        }
        ForceUpdateDependenciesDeep();
    }

    private void CustomDestroy()
    {
        var gun = (IDestroyable)Gun;
        gun.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

    private void OnReactToDamageReceived(decimal damage, IDamageArea area)
    {
        if (area is Enemy enemy)
        {
            Velocity += enemy.Velocity.NormalizedOrZero() * enemy.KnockbackVelocity;
        }

        HealthBar.ProgressPercentage = (float)(100 * CurrentHealth / MaxHealth);
    }

    private void HandleInput()
    {
        if (AimInMeleeRange)
        {
            if (!_meleeLastFrame)
            {
                MeleeWeapon.Equip();
                Gun.Unequip();
            }
                
            if (GameplayInputDevice.Attack.WasJustPressed)
            {
                MeleeWeapon.BeginAttack();
            }
            else if (GameplayInputDevice.Attack.WasJustReleased)
            {
                MeleeWeapon.EndAttack();
            }

            _meleeLastFrame = true;
        }
        else
        {
            if (_meleeLastFrame)
            {
                Gun.Equip();
                MeleeWeapon.Unequip();
            }
                
            if (GameplayInputDevice.Attack.WasJustPressed)
            {
                Gun.BeginFire();
            }
            else if (GameplayInputDevice.Attack.WasJustReleased)
            {
                Gun.EndFire();
            }

            _meleeLastFrame = false;
        }
            
        if (GameplayInputDevice.Dash.WasJustPressed)
        {
            var dir = GameplayInputDevice.Movement.GetNormalizedPositionOrZero().ToVec3();
            if (dir != Vector3.Zero)
            {
                Position += dir * DashDistance;
            }
        }

        if (GameplayInputDevice.Reload.WasJustPressed)
        {
            Gun.Reload();
        }
    }
}