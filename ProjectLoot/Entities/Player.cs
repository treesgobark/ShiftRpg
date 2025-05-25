using System.Diagnostics;
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
using ProjectLoot.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

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

    public StateMachine States { get; protected set; }

    private void CustomInitialize()
    {
        InitializeInputs();
        InitializeComponents();
        InitializeControllers();
        InitializeHandlers();
        InitializeChildren();
        GiveWeapons();
    }

    private void GiveWeapons()
    {
        PickUpWeapon(GlobalContent.MeleeWeaponData[MeleeWeaponData.Sword]);
        PickUpWeapon(GlobalContent.MeleeWeaponData[MeleeWeaponData.Fists]);
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
        States = new StateMachine();
        States.Add(new Unarmed(this, States, FrbTimeManager.Instance));
        States.Add(new MeleeWeaponMode(this, States, FrbTimeManager.Instance));
        States.Add(new GunMode(this, States, FrbTimeManager.Instance));
        States.Add(new Dashing(this, States, FrbTimeManager.Instance));
        States.Add(new Guarding(this, States, FrbTimeManager.Instance));
        States.InitializeStartingState<Unarmed>();
    }

    private void InitializeHandlers()
    {
        EffectsComponent.AddHandler<HitstopEffect>(new PlayerHitstopHandler(EffectsComponent, HitstopComponent, FrbTimeManager.Instance, PlayerSpriteComponent));
        EffectsComponent.AddHandler<AttackEffect>(new AttackHandler(EffectsComponent, HealthComponent, FrbTimeManager.Instance));
        EffectsComponent.AddHandler<HealthReductionEffect>(new HealthReductionHandler(EffectsComponent, HealthComponent, FrbTimeManager.Instance, HitstopComponent));
        EffectsComponent.AddHandler<HealthReductionEffect>(new DamageNumberHandler(EffectsComponent, TransformComponent));
        EffectsComponent.AddHandler<KnockbackEffect>(new KnockbackHandler(EffectsComponent, TransformComponent));
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
        
        States.DoCurrentStateActivity();
        
        if (HitstopComponent.IsStopped)
        {
            MeleeWeaponComponent.DoMinimumStateActivity();
        }
        else
        {
            GunComponent.Activity();
            MeleeWeaponComponent.Activity();
        }
        
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
        States.Uninitialize();
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
                model = new FistsModel(meleeWeapon, MeleeWeaponComponent, EffectsComponent);
                break;
            case { Name: MeleeWeaponData.Sword }:
            case { Name: MeleeWeaponData.Dagger }:
                model = new SwordModel(meleeWeapon, MeleeWeaponComponent, EffectsComponent);
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