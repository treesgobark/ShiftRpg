using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;
using GumCoreShared.FlatRedBall.Embedded;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.Effects.Handlers;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;
using ShiftRpg.Models;

namespace ShiftRpg.Entities;

public partial class Player : ITakesDamage, IWeaponHolder, ITakesKnockback
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public IWeaponCache<IGun, IGunInputDevice> GunCache { get; set; }
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> MeleeWeaponCache { get; set; }
    public float LastMeleeRotation { get; set; }

    // Implement IHasControllers
    public StateMachine StateMachine { get; protected set; }

    // Implement IEffectReceiver
    IReadOnlyEffectHandlerCollection IReadOnlyEffectReceiver.HandlerCollection => HandlerCollection;
    public IEffectHandlerCollection HandlerCollection { get; protected set; }

    // Implement ITakesDamage
    public IStatModifierCollection<float> DamageModifiers { get; } = new StatModifierCollection<float>();
    public Team Team { get; set; }
    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; } =
        new List<(Guid EffectId, double EffectTime)>();
    public float CurrentHealth { get; set; }
    public double LastDamageTime { get; set; }

    public IEffectBundle ModifyTargetEffects(IEffectBundle effects)
    {
        foreach (object effect in effects)
            if (effect is DamageEffect damageEffect && damageEffect.Source.Contains(SourceTag.Gun))
                damageEffect.AdditiveIncreases.Add(1f);

        return effects;
    }

    public void SetInputEnabled(bool isEnabled)
    {
        InputEnabled = isEnabled;
    }

    private void CustomInitialize()
    {
        InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
        InitializeWeapons();
        InitializeControllers();
        PositionedObjectGueWrapper hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        Team = Team.Player;
        CurrentHealth = MaxHealth;
        HandlerCollection = new EffectHandlerCollection(this);
        HealthBar.Reset();
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new MeleeMode(this, StateMachine));
        StateMachine.Add(new GunMode(this, StateMachine));
        StateMachine.InitializeStartingState<MeleeMode>();
    }

    private void InitializeWeapons()
    {
        GameplayInputDevice = new GameplayInputDevice(InputDevice, this);

        DefaultGun gun = DefaultGunFactory.CreateNew();

        gun.RelativeX = 10;
        gun.AttachTo(this);
        gun.Team = Team.Player;
        gun.Holder = this;

        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, new GunInputDevice(GameplayInputDevice));
        GunCache.Add(gun);

        DefaultSword melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Team = Team.Player;
        melee.Holder = this;

        MeleeWeaponCache = new WeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice>(ZeroMeleeWeapon.Instance,
            new MeleeWeaponInputDevice(GameplayInputDevice));
        MeleeWeaponCache.Add(melee);
    }

    private void CustomActivity()
    {
        // HandlePersistentEffects();
        StateMachine.DoCurrentStateActivity();
    }

    private void CustomDestroy()
    {
        GunCache.Destroy();
        MeleeWeaponCache.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

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
        foreach (object effect in effects)
            if (effect is DamageEffect damage && damage.Source.IsContainedIn(SourceTag.Gun))
            {
                // damage.AdditiveIncreases.Add(1);
            }
    }
}