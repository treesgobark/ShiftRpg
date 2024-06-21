using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;
using GumCoreShared.FlatRedBall.Embedded;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities;

public partial class Player : IWeaponHolder
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public float LastMeleeRotation { get; set; }
    
    public HealthComponent Health { get; private set; }
    public EffectsComponent Effects { get; private set; }
    public WeaponsComponent Weapons { get; private set; }

    public StateMachine StateMachine { get; protected set; }

    private void CustomInitialize()
    {
        InitializeTopDownInput(InputManager.Keyboard);
        GameplayInputDevice = new GameplayInputDevice(InputDevice, this, MeleeAimThreshold);
        
        Health = new HealthComponent(MaxHealth, HealthBar);
        Effects = new EffectsComponent { Team = Team.Player };
        Weapons = new WeaponsComponent(GameplayInputDevice, Team.Player, this, this);
        
        InitializeControllers();
        InitializeHandlers();
        PositionedObjectGueWrapper hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        HealthBar.Reset();
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new MeleeMode(this, StateMachine));
        StateMachine.Add(new GunMode(this, StateMachine));
        StateMachine.InitializeStartingState<MeleeMode>();
    }

    private void InitializeHandlers()
    {
        Effects.HandlerCollection.Add(new DamageHandler(Health, Effects, this));
    }

    private void CustomActivity()
    {
        // HandlePersistentEffects();
        Weapons.Activity();
        StateMachine.DoCurrentStateActivity();
    }

    private void CustomDestroy()
    {
        Weapons.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }
    
    #region IWeaponHolder

    IEffectsComponent IWeaponHolder.Effects => Effects;
    
    public IEffectBundle ModifyTargetEffects(IEffectBundle effects)
    {
        return effects;
    }

    public IEffectsComponent EffectsComponent => Effects;
    
    #endregion

}