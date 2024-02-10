using System;
using System.Collections.Generic;
using System.Linq;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
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

public partial class Player : IHasControllers<Player, PlayerController>, ITakesDamage
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
        Team                                    = Team.Player;
        CurrentHealth                           = MaxHealth;
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
        gun.ModifyTargetEffects           = ModifyOutgoingEffects;
        gun.Team                          = Team.Player;
        
        Gun = gun;
    }

    private void InitializeMeleeWeapon()
    {
        var melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Owner                         = this;
        melee.ApplyHolderEffects            = HandleEffects;
        melee.ModifyTargetEffects           = ModifyOutgoingEffects;
        melee.ParentRotationChangesRotation = true;
        melee.Team                          = Team.Player;

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

    public void HandleEffects(IReadOnlyList<IEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (RecentEffects.Any(t => t.EffectId == effect.EffectId))
            {
                continue;
            }
            
            effect.HandleStandardDamage(this)
                .HandleStandardKnockback(this);
        }
    }

    public void ModifyOutgoingEffects(IReadOnlyList<IEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (effect is DamageEffect damage && damage.Source.IsSubsetOf(SourceTag.Gun))
            {
                damage.AdditiveIncreases.Add(1);
            }
        }
    }

    public Team Team { get; set; }
    
    // Implement IHasControllers
    
    public ControllerCollection<Player, PlayerController> Controllers { get; protected set; }
    
    // Implement ITakesDamage
    
    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; } = new List<(Guid AttackId, double AttackTime)>();
    public float CurrentHealthPercentage => 100f * CurrentHealth / MaxHealth;
    public double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(LastDamageTime);
    public bool IsInvulnerable => TimeSinceLastDamage < InvulnerabilityTimeAfterDamage;
    public int CurrentHealth { get; set; }
    public double LastDamageTime { get; set; }
    public void TakeDamage(int damage)
    {
        CurrentHealth                -= damage;
        HealthBar.ProgressPercentage =  CurrentHealthPercentage;
    }
}