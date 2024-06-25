using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Input;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.Handlers;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities;

public partial class Player
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public float LastMeleeRotation { get; set; }
    
    public EffectsComponent Effects { get; private set; }
    public TransformComponent Transform { get; private set; }
    public HealthComponent Health { get; private set; }
    public HitstopComponent Hitstop { get; private set; }
    public HitstunComponent Hitstun { get; private set; }
    public GunComponent Gun { get; private set; }
    public MeleeWeaponComponent MeleeWeapon { get; private set; }
    public SpriteComponent Sprite { get; private set; }

    public StateMachine StateMachine { get; protected set; }

    private void CustomInitialize()
    {
        InitializeInputs();
        InitializeComponents();
        InitializeControllers();
        InitializeHandlers();
        
        AimThresholdCircle.AttachTo(GameplayCenter);
        DirectionIndicator.AttachTo(GameplayCenter);
        GuardSprite.AttachTo(GameplayCenter);
    }

    private void InitializeInputs()
    {
        InitializeTopDownInput(InputManager.Keyboard);
        GameplayInputDevice = new GameplayInputDevice(InputDevice, GameplayCenter, MeleeAimThreshold);
    }

    private void InitializeComponents()
    {
        Effects = new EffectsComponent { Team = Team.Player };
        Transform = new TransformComponent(this);
        Health = new HealthComponent(MaxHealth);
        Hitstop = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
        Hitstun = new HitstunComponent();
        Gun = new GunComponent(new GunInputDevice(GameplayInputDevice));
        MeleeWeapon = new MeleeWeaponComponent(new MeleeWeaponInputDevice(GameplayInputDevice));
        Sprite = new SpriteComponent(PlayerSprite);
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new MeleeMode(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new GunMode(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Dashing(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Guarding(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.InitializeStartingState<MeleeMode>();
    }

    private void InitializeHandlers()
    {
        Effects.HandlerCollection.Add(new HitstopHandler(Effects, Hitstop, Transform, FrbTimeManager.Instance, Sprite));
        Effects.HandlerCollection.Add(new DamageHandler(Effects, Health, Transform, FrbTimeManager.Instance, this));
        Effects.HandlerCollection.Add(new KnockbackHandler(Effects, Transform, Hitstop));
        Effects.HandlerCollection.Add(new GunHandler(Gun, Hitstun, Hitstop));
        Effects.HandlerCollection.Add(new MeleeWeaponHandler(MeleeWeapon, Hitstun, Hitstop));
    }

    private void CustomActivity()
    {
        Effects.Activity();
        
        PlayerSprite.ForceUpdateDependenciesDeep();
        StateMachine.DoCurrentStateActivity();
    }

    private void CustomDestroy()
    {
        Gun.Cache.Destroy();
        MeleeWeapon.Cache.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }
}