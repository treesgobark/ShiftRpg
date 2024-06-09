using System;
using System.Collections.Generic;
using System.Linq;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall;
using FlatRedBall.Entities;
using FlatRedBall.Graphics;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.Effects.Handlers;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;
using ShiftRpg.Models;

namespace ShiftRpg.Entities;

public partial class Player : ITakesDamage, IWeaponHolder
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
        HandlerCollection = new EffectHandlerCollection(this);
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new Idle(this, StateMachine));
        StateMachine.Add(new MeleeMode(this, StateMachine));
        StateMachine.Add(new GunMode(this, StateMachine));
        StateMachine.InitializeStartingState<MeleeMode>();
    }

    private void InitializeGun()
    {
        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, GunInputDevice);
        
        var gun = DefaultGunFactory.CreateNew();
            
        gun.RelativeX = 10;
        gun.AttachTo(this);
        gun.Team   = Team.Player;
        gun.Holder = this;

        GunCache.Add(gun);
    }

    private void InitializeMeleeWeapon()
    {
        MeleeWeaponCache = new WeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice>(ZeroMeleeWeapon.Instance, MeleeInputDevice);
        
        var melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Team   = Team.Player;
        melee.Holder = this;

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
        // HandlePersistentEffects();
        StateMachine.DoCurrentStateActivity();
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

    IReadOnlyEffectHandlerCollection IReadOnlyEffectReceiver.HandlerCollection => HandlerCollection;
    public IEffectHandlerCollection HandlerCollection { get; protected set; }
    // public IList<IPersistentEffect> PersistentEffects { get; } = new List<IPersistentEffect>();
    public Team Team { get; set; }

    // public void HandlePersistentEffects()
    // {
    //     List<IEffect> effects = [];
    //
    //     for (var i = PersistentEffects.Count - 1; i >= 0; i--)
    //     {
    //         var effect = PersistentEffects[i];
    //         if (effect is DamageOverTimeEffect { ShouldApply: true } dot)
    //         {
    //             effects.Add(dot.GetDamageEffect());
    //             if (dot.RemainingTicks <= 0)
    //             {
    //                 PersistentEffects.RemoveAt(i);
    //             }
    //         }
    //     }
    //
    //     HandleEffects(effects);
    // }
    //
    // public void HandleEffects(IEffectBundle effects)
    // {
    //     foreach (var effect in effects)
    //     {
    //         if (RecentEffects.Any(t => t.EffectId == effect.EffectId))
    //         {
    //             continue;
    //         }
    //         
    //         effect.HandleStandardDamage(this)
    //             .HandleStandardKnockback(this)
    //             .HandleStandardPersistentEffect(this);
    //     }
    // }

    public void ModifyOutgoingEffects(IEffectBundle effects)
    {
        foreach (var effect in effects)
        {
            if (effect is DamageEffect damage && damage.Source.IsContainedIn(SourceTag.Gun))
            {
                // damage.AdditiveIncreases.Add(1);
            }
        }
    }
    
    // Implement IHasControllers
    
    public StateMachine StateMachine { get; protected set; }
    
    // Implement ITakesDamage
    
    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; } = new List<(Guid EffectId, double EffectTime)>();
    public float CurrentHealth { get; set; }
    public double LastDamageTime { get; set; }

    public IEffectBundle ModifyTargetEffects(IEffectBundle effects)
    {
        foreach (object effect in effects)
        {
            if (effect is DamageEffect damageEffect && damageEffect.Source.Contains(SourceTag.Gun))
            {
                damageEffect.AdditiveIncreases.Add(1f);
            }
        }

        return effects;
    }

    public void SetInputEnabled(bool isEnabled)
    {
        InputEnabled = isEnabled;
    }
}