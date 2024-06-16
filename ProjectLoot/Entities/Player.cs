using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;
using GumCoreShared.FlatRedBall.Embedded;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.GumRuntimes;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Entities;

public partial class Player
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public IWeaponCache<IGun, IGunInputDevice> GunCache { get; set; }
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> MeleeWeaponCache { get; set; }
    public float LastMeleeRotation { get; set; }
    
    public HealthComponent Health { get; private set; }
    public EffectsComponent Effects { get; private set; }

    public StateMachine StateMachine { get; protected set; }

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
        Health = new HealthComponent(MaxHealth, HealthBar);
        
        Effects = new EffectsComponent { Team = Team.Player };
        
        InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
        InitializeWeapons();
        InitializeControllers();
        PositionedObjectGueWrapper hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
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
        gun.Team = Effects.Team;
        // gun.Holder = this;
        gun.Holder = ZeroWeaponHolder.Instance;

        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, new GunInputDevice(GameplayInputDevice));
        GunCache.Add(gun);

        DefaultSword melee = DefaultSwordFactory.CreateNew();

        melee.AttachTo(this);
        melee.Team = Effects.Team;
        // melee.Holder = this;
        melee.Holder = ZeroWeaponHolder.Instance;

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