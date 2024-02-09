using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Entities;
using FlatRedBall.Graphics;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Player;
using ShiftRpg.Effects;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities;

public partial class Player : IHasControllers<Player, PlayerController>, IEffectReceiver
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public IGunInputDevice GunInputDevice { get; set; }
    public IMeleeWeaponInputDevice MeleeInputDevice { get; set; }
    
    public IGun Gun { get; set; }
    public IMeleeWeapon MeleeWeapon { get; set; }
    
    public bool AimInMeleeRange => GameplayInputDevice.Aim.Magnitude < 1;
    public float LastMeleeRotation { get; set; }

    private void CustomInitialize()
    {
        InitializeGun();
        InitializeMeleeWeapon();
        InitializeControllers();
        var hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
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
        gun.ApplyHolderEffects            = HandleEffects;
            
        Gun = gun;
    }

    private void InitializeMeleeWeapon()
    {
        var melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Owner                         = this;
        melee.ApplyHolderEffects            = HandleEffects;
        melee.ParentRotationChangesRotation = true;

        MeleeWeapon = melee;
    }

    partial void CustomInitializeTopDownInput()
    {
        GameplayInputDevice = new GameplayInputDevice(InputDevice, this);
        GunInputDevice      = new GunInputDevice(GameplayInputDevice);
        MeleeInputDevice    = new MeleeWeaponInputDevice(GameplayInputDevice);
    }

    private void CustomActivity()
    {
        Controllers.DoCurrentControllerActivity();
    }

    private void CustomDestroy()
    {
        var gun = (IDestroyable)Gun;
        gun.Destroy();
        var melee = (IDestroyable)Gun;
        melee.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName) { }
    
    // Implement IEffectReceiver

    public void HandleEffects(IEnumerable<object> effects)
    {
        foreach (object effect in effects)
        {
            switch (effect)
            {
                case KnockbackEffect knockbackEffect: Handle(knockbackEffect);
                    break;
                case DamageEffect damageEffect: Handle(damageEffect);
                    break;
            }
        }
    }

    private void Handle(KnockbackEffect knockbackEffect)
    {
        Velocity += knockbackEffect.KnockbackVector;
    }

    private void Handle(DamageEffect damageEffect)
    {
        // CurrentHealth -= (decimal)damageEffect.Damage;
    }
    
    // Implement IHasControllers
    
    public ControllerCollection<Player, PlayerController> Controllers { get; protected set; }
}