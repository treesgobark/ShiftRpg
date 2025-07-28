using System.Collections.Generic;
using System.Diagnostics;
using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.States;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;
using ProjectLoot.Models.SpearModel;
using SwordModel = ProjectLoot.Models.SwordModel;

namespace ProjectLoot.Entities;

public partial class Player
{
    public GameplayInputDevice GameplayInputDevice { get; set; }
    private float LastMeleeRotation { get; set; }
    private double BobTimer { get; set; }

    public EffectsComponent EffectsComponent { get; private set; }
    public TransformComponent TransformComponent { get; private set; }
    public HealthComponent HealthComponent { get; private set; }
    public HitstopComponent HitstopComponent { get; private set; }
    public HitstunComponent HitstunComponent { get; private set; }
    public GunComponent GunComponent { get; private set; }
    public MeleeWeaponComponent MeleeWeaponComponent { get; private set; }
    public SpriteComponent PlayerSpriteComponent { get; private set; }

    public StateMachineManager StateMachines { get; private set; }
    
    public ITimeManager HitstopTimeManager { get; private set; }

    private void CustomInitialize()
    {
        InitializeInputs();
        InitializeComponents();
        InitializeTimeManager();
        InitializeControllers();
        InitializeHandlers();
        InitializeChildren();
        GiveWeapons();
    }

    private void InitializeTimeManager()
    {
        HitstopTimeManager = new HitstopAwareTimeManager(HitstopComponent);
    }

    private void GiveWeapons()
    {
        PickUpWeapon(GlobalContent.MeleeWeaponData[MeleeWeaponData.Sword]);
        PickUpWeapon(GlobalContent.MeleeWeaponData[MeleeWeaponData.Fists]);
        PickUpWeapon(GlobalContent.MeleeWeaponData[MeleeWeaponData.Spear]);
        // PickUpWeapon(GlobalContent.GunData[GunData.Rifle]);
    }

    private void InitializeInputs()
    {
        // InitializeTopDownInput(InputManager.Keyboard);
        GameplayInputDevice = new GameplayInputDevice(InputDevice, GameplayCenter, MeleeAimThreshold);
    }

    private void InitializeComponents()
    {
        EffectsComponent      = new EffectsComponent { Team = Team.Player };
        TransformComponent    = new TransformComponent(this, this);
        HealthComponent       = new HealthComponent(MaxHealth);
        HitstopComponent      = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
        HitstunComponent      = new HitstunComponent();
        GunComponent          = new GunComponent(Team.Player, GameplayInputDevice, GunSprite, GameplayCenter);
        MeleeWeaponComponent  = new MeleeWeaponComponent(Team.Player, GameplayInputDevice, this, GameplayCenter, MeleeWeaponSprite, PlayerSprite, GameplayCenter);
        PlayerSpriteComponent = new SpriteComponent(PlayerSprite);
    }

    private void InitializeControllers()
    {
        StateMachines = new();
        
        var states = new StateMachine();
        states.Add(new Unarmed(this, states, FrbTimeManager.Instance));
        states.Add(new MeleeWeaponMode(this, states, FrbTimeManager.Instance));
        states.Add(new GunMode(this, states, FrbTimeManager.Instance));
        states.Add(new Dashing(this, states, FrbTimeManager.Instance));
        states.Add(new Guarding(this, states, FrbTimeManager.Instance));
        
        StateMachines.Add<Unarmed>(states);
    }

    private void InitializeHandlers()
    {
        EffectsComponent.AddHandler(new PlayerHitstopHandler(EffectsComponent, HitstopComponent, FrbTimeManager.Instance, PlayerSpriteComponent));
        EffectsComponent.AddHandler(new AttackHandler(EffectsComponent, HealthComponent, FrbTimeManager.Instance));
        EffectsComponent.AddHandler(new HealthReductionHandler(EffectsComponent, HealthComponent, FrbTimeManager.Instance));
        EffectsComponent.AddHandler(new DamageNumberHandler(EffectsComponent, TransformComponent));
        EffectsComponent.AddHandler(new KnockbackHandler(EffectsComponent, TransformComponent));
    }

    private void InitializeChildren()
    {
        AimThresholdCircle.AttachTo(GameplayCenter);
        DirectionIndicator.AttachTo(GameplayCenter);
        GuardSprite.AttachTo(GameplayCenter);
        MeleeWeaponSprite.AttachTo(GameplayCenter);
        GunSprite.AttachTo(GameplayCenter);
        ReticleSprite.AttachTo(GameplayCenter);
        TargetLineSprite.AttachTo(GameplayCenter);
        
        PlayerSprite.AttachTo(SpriteHolder);
        EyesSprite.AttachTo(SpriteHolder);
    }

    private void CustomActivity()
    {
        EffectsComponent.Activity();

        PlayerSprite.ForceUpdateDependenciesDeep();
        
        StateMachines.DoAllStateMachineActivity();
        
        GunComponent.Activity();
        MeleeWeaponComponent.Activity();
        
        UpdateReticlePosition();

        if (XVelocity != 0)
        {
            Debug.WriteLine($"Player@{TimeManager.CurrentFrame}:XVelocity:{XVelocity}:XAcceleration:{XAcceleration}:InputX:{MovementInput.X}:InputY:{MovementInput.Y}");
        }
    }

    private void UpdateReticlePosition()
    {
        var     mousePos                  = new Vector2(InputManager.Mouse.WorldXAt(Z), InputManager.Mouse.WorldYAt(Z));
        Vector2 fromGameplayCenterToMouse = GameplayCenter.Position.ToVector2().GetVectorTo(mousePos);
        
        ReticleSprite.RelativeX = fromGameplayCenterToMouse.Length();

        // var length = ReticleSprite.RelativeX - GunSprite.RelativeX - GunSprite.Width / 2f - ReticleSprite.Width / 2f;
        var length = ReticleSprite.RelativeX - ReticleSprite.Width / 2f;
        TargetLineSprite.Width             = length;
        TargetLineSprite.LeftTexturePixel  = -length / 2f;
        TargetLineSprite.RightTexturePixel = length / 2f;
        TargetLineSprite.FlipHorizontal    = true;

        // TargetLineSprite.RelativeX = length / 2 + (GunSprite.RelativeX + GunSprite.Width / 2f);
        TargetLineSprite.RelativeX = length / 2;
    }

    private void CustomDestroy()
    {
        StateMachines.ShutDown();
    }

    private static void CustomLoadStaticContent(string contentManagerName) { }

    public bool PickUpWeapon(GunData gun)
    {
        var gunModel = new StandardGunModel(gun, GunComponent, GunComponent, EffectsComponent);
        GunComponent.Add(gunModel);
        return true;
    }

    public bool PickUpWeapon(MeleeWeaponData meleeWeapon)
    {
        IMeleeWeaponModel model;

        switch (meleeWeapon)
        {
            case { Name: MeleeWeaponData.Fists }:
                model = new FistsModel(meleeWeapon, MeleeWeaponComponent, EffectsComponent, HitstopTimeManager);
                break;
            case { Name: MeleeWeaponData.Sword }:
            case { Name: MeleeWeaponData.Dagger }:
                model = new SwordModel(meleeWeapon, MeleeWeaponComponent, EffectsComponent, HitstopTimeManager);
                break;
            case { Name: MeleeWeaponData.Spear }:
                model = new SpearModel(meleeWeapon, MeleeWeaponComponent, EffectsComponent, HitstopTimeManager);
                break;
            default:
                throw new ArgumentException($"Unrecognized weapon type: {meleeWeapon.Name}");
        }
        
        MeleeWeaponComponent.Add(model);
        
        return true;
    }

    private void HandleBobbing()
    {
        SpriteHolder.RelativeY =  1f * (float)Math.Sin(2f * BobTimer);
        BobTimer               += FrbTimeManager.Instance.GameTimeSinceLastFrame.TotalSeconds;
    }
}

public class StateMachineManager : IStateMachineManager
{
    private readonly List<IStateMachine> _stateMachines = [];

    public void Add(IStateMachine stateMachine)
    {
        _stateMachines.Add(stateMachine);
    }

    public void Add<TSearch>(IStateMachine stateMachine, bool isExact = false) where TSearch : IState
    {
        stateMachine.SetStartingState<TSearch>(isExact);
        _stateMachines.Add(stateMachine);
    }

    public void DoAllStateMachineActivity()
    {
        // evaluate exit conditions for each machine's current state
        // determine if any of the new states contain a dependency on another state machine
        // send each 
        
        foreach (var stateMachine in _stateMachines)
        {
            stateMachine.DoCurrentStateActivity();
        }
    }

    public void ShutDown()
    {
        foreach (IStateMachine stateMachine in _stateMachines)
        {
            stateMachine.ShutDown();
        }
        
        _stateMachines.Clear();
    }
}