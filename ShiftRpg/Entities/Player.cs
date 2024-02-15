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
using ShiftRpg.Models;

namespace ShiftRpg.Entities;

public partial class Player : IHasControllers<Player, PlayerController>, ITakesDamage
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public IGunInputDevice GunInputDevice { get; set; }
    public IMeleeWeaponInputDevice MeleeInputDevice { get; set; }
    
    public IWeaponCache<IGun, IGunInputDevice> GunCache { get; set; }
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> MeleeWeaponCache { get; set; }
    public IGun Gun => GunCache.CurrentWeapon;
    public IMeleeWeapon MeleeWeapon => MeleeWeaponCache.CurrentWeapon;
    
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
        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, GunInputDevice);
        
        var gun = DefaultGunFactory.CreateNew();
            
        gun.RelativeX = 10;
        gun.AttachTo(this);
        gun.ApplyHolderEffects            = HandleEffects;
        gun.ModifyTargetEffects           = ModifyOutgoingEffects;
        gun.Team                          = Team.Player;

        GunCache.Add(gun);
    }

    private void InitializeMeleeWeapon()
    {
        MeleeWeaponCache = new WeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice>(ZeroMeleeWeapon.Instance, MeleeInputDevice);
        
        var melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Owner                         = this;
        melee.ApplyHolderEffects            = HandleEffects;
        melee.ModifyTargetEffects           = ModifyOutgoingEffects;
        melee.Team                          = Team.Player;

        MeleeWeaponCache.Add(melee);
    }

    partial void CustomInitializeTopDownInput()
    {
        GameplayInputDevice          = new GameplayInputDevice(InputDevice, this);
        
        GunInputDevice               = new GunInputDevice(GameplayInputDevice);
        if (GunCache != null) GunCache.InputDevice = GunInputDevice;

        MeleeInputDevice             = new MeleeWeaponInputDevice(GameplayInputDevice);
        if (MeleeWeaponCache != null) MeleeWeaponCache.InputDevice = MeleeInputDevice;
    }

    private void CustomActivity()
    {
        HandlePersistentEffects();
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

    public IList<IPersistentEffect> PersistentEffects { get; } = new List<IPersistentEffect>();
    public Team Team { get; set; }

    public void HandlePersistentEffects()
    {
        List<IEffect> effects = [];

        for (var i = PersistentEffects.Count - 1; i >= 0; i--)
        {
            var effect = PersistentEffects[i];
            if (effect is DamageOverTimeEffect { ShouldApply: true } dot)
            {
                effects.Add(dot.GetDamageEffect());
                if (dot.RemainingTicks <= 0)
                {
                    PersistentEffects.RemoveAt(i);
                }
            }
        }

        HandleEffects(effects);
    }

    public void HandleEffects(IReadOnlyList<IEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (RecentEffects.Any(t => t.EffectId == effect.EffectId))
            {
                continue;
            }
            
            effect.HandleStandardDamage(this)
                .HandleStandardKnockback(this)
                .HandleStandardPersistentEffect(this);
        }
    }

    public void ModifyOutgoingEffects(IReadOnlyList<IEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (effect is DamageEffect damage && damage.Source.IsSubsetOf(SourceTag.Gun))
            {
                // damage.AdditiveIncreases.Add(1);
            }
        }
    }
    
    // Implement IHasControllers
    
    public ControllerCollection<Player, PlayerController> Controllers { get; protected set; }
    
    // Implement ITakesDamage
    
    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; } = new List<(Guid EffectId, double EffectTime)>();
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