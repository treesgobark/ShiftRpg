using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Entities;

public partial class Player
{
    public IGameplayInputDevice GameplayInputDevice { get; set; }
    public float LastMeleeRotation { get; set; }

    public EffectsComponent EffectsComponent { get; private set; }
    public TransformComponent TransformComponent { get; private set; }
    public HealthComponent HealthComponent { get; private set; }
    public HitstopComponent HitstopComponent { get; private set; }
    public HitstunComponent HitstunComponent { get; private set; }
    public GunComponent GunComponent { get; private set; }
    public MeleeWeaponComponent MeleeWeaponComponent { get; private set; }
    public SpriteComponent PlayerSpriteComponent { get; private set; }

    public StateMachine StateMachine { get; protected set; }

    private void CustomInitialize()
    {
        InitializeInputs();
        InitializeComponents();
        InitializeControllers();
        InitializeHandlers();
        InitializeChildren();
    }

    private void InitializeInputs()
    {
        InitializeTopDownInput(InputManager.Keyboard);
        GameplayInputDevice = new GameplayInputDevice(InputDevice, GameplayCenter, MeleeAimThreshold);
    }

    private void InitializeComponents()
    {
        EffectsComponent      = new EffectsComponent { Team = Team.Player };
        TransformComponent    = new TransformComponent(this);
        HealthComponent       = new HealthComponent(MaxHealth);
        HitstopComponent      = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
        HitstunComponent      = new HitstunComponent();
        GunComponent          = new GunComponent(GunSprite, Team.Player, GameplayInputDevice);
        MeleeWeaponComponent  = new MeleeWeaponComponent();
        PlayerSpriteComponent = new SpriteComponent(PlayerSprite);
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new Unarmed(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new MeleeWeaponMode(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new GunMode(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Dashing(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Guarding(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.InitializeStartingState<Unarmed>();
    }

    private void InitializeHandlers()
    {
        EffectsComponent.HandlerCollection.Add<HitstopEffect>(new HitstopHandler(EffectsComponent, HitstopComponent, TransformComponent, FrbTimeManager.Instance, PlayerSpriteComponent));
        EffectsComponent.HandlerCollection.Add<DamageEffect>(new DamageHandler(EffectsComponent, HealthComponent, TransformComponent, FrbTimeManager.Instance, this));
        EffectsComponent.HandlerCollection.Add<KnockbackEffect>(new KnockbackHandler(EffectsComponent, TransformComponent, HitstopComponent));
    }

    private void InitializeChildren()
    {
        AimThresholdCircle.AttachTo(GameplayCenter);
        DirectionIndicator.AttachTo(GameplayCenter);
        GuardSprite.AttachTo(GameplayCenter);
        GunSprite.AttachTo(GameplayCenter);
        ReticleSprite.AttachTo(GameplayCenter);
        TargetLineSprite.AttachTo(GameplayCenter);
    }

    private void CustomActivity()
    {
        EffectsComponent.Activity();

        PlayerSprite.ForceUpdateDependenciesDeep();

        StateMachine.DoCurrentStateActivity();
        GunComponent.Activity();
        PlayerSprite.AnimateSelf(TimeManager.CurrentScreenTime);
        UpdateReticlePosition();
    }

    private void UpdateReticlePosition()
    {
        var     mousePos                  = new Vector2(InputManager.Mouse.WorldXAt(Z), InputManager.Mouse.WorldYAt(Z));
        Vector2 fromGameplayCenterToMouse = GameplayCenter.Position.ToVector2().GetVectorTo(mousePos);
        
        ReticleSprite.RelativeX = fromGameplayCenterToMouse.Length();

        var length = ReticleSprite.RelativeX - GunSprite.RelativeX - GunSprite.Width / 2f;
        TargetLineSprite.Width             = length;
        TargetLineSprite.RightTexturePixel = length;

        TargetLineSprite.RelativeX = length / 2 + (GunSprite.RelativeX + GunSprite.Width / 2f);
    }

    private void CustomDestroy() { }

    private static void CustomLoadStaticContent(string contentManagerName) { }

    public bool PickUpWeapon(GunData gun)
    {
        var gunModel = new StandardGunModel(gun, GunComponent, GunComponent);
        GunComponent.Add(gunModel);
        return true;
    }
}