using ANLG.Utilities.Core.Constants;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities;

public partial class Gun
{
    public StateMachine StateMachine { get; private set; }

    public EffectsComponent Effects { get; private set; }

    public IGunModel? GunModel { get; set; }

    public SourceTag Source { get; set; } = SourceTag.Gun;

    // Implement IGun

    // public IWeaponHolder Holder { get; set; }

    public IEffectBundle TargetHitEffects
    {
        get
        {
            var effects = new EffectBundle(~Effects.Team, Source);

            effects.AddEffect(new DamageEffect(~Effects.Team, Source, GunModel.GunData.Damage));
            effects.AddEffect(new KnockbackEffect(~Effects.Team, Source, 200, this.GetRotationZ(),
                                                  KnockbackBehavior.Replacement));
            effects.AddEffect(new ShatterDamageEffect(~Effects.Team, Source, 3));
            effects.AddEffect(new HitstopEffect(~Effects.Team, Source, TimeSpan.FromMilliseconds(50)));

            // return Holder.ModifyTargetEffects(effects);
            return effects;
        }
    }

    public IEffectBundle HolderHitEffects { get; set; } = EffectBundle.Empty;
    public IGunInputDevice InputDevice { get; set; } = ZeroGunInputDevice.Instance;

    /// <summary>
    ///     Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    ///     This method is called when the Entity is added to managers. Entities which are instantiated but not
    ///     added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        ParentRotationChangesRotation = true;

        InitializeComponent();

        InitializeStates();
    }

    private void InitializeComponent()
    {
        Effects = new EffectsComponent();
    }

    private void InitializeStates()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new Ready(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Recovery(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Reloading(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.InitializeStartingState<Ready>();
    }

    private void CustomActivity()
    {
        StateMachine.DoCurrentStateActivity();
        SetSpriteFlip();
    }

    private void SetSpriteFlip()
    {
        Rotation rotation = Rotation.FromRadians(RotationZ);
        if (rotation.CondensedDegrees is > -90 and < 90)
        {
            SpriteInstance.FlipVertical = false;
        }
        else
        {
            SpriteInstance.FlipVertical = true;
        }
    }

    private void CustomDestroy()
    {
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

    public void Equip(IGunInputDevice input, IGunModel gunModel)
    {
        InputDevice                     = input;
        SpriteInstance.CurrentChainName = gunModel.GunData.GunName;
        SpriteInstance.Visible          = true;

        GunModel = gunModel;
    }

    public void Unequip()
    {
        InputDevice            = ZeroGunInputDevice.Instance;
        SpriteInstance.Visible = false;
    }

    private void Fire()
    {
        var dir = Vector2ExtensionMethods.FromAngle(Parent.RotationZ).NormalizedOrZero().ToVector3();
        if (dir == Vector3.Zero)
        {
            return;
        }

        Bullet? proj = BulletFactory.CreateNew(Position);
        // proj.InitializeProjectile(GunModel.GunData.ProjectileRadius, dir * GunModel.GunData.ProjectileSpeed, TargetHitEffects,
        //                           HolderHitEffects, Holder, ~Effects.Team);
        proj.InitializeProjectile(GunModel.GunData.ProjectileRadius, dir * GunModel.GunData.ProjectileSpeed, TargetHitEffects,
                                  HolderHitEffects, ~Effects.Team);

        var effects = new EffectBundle(Effects.Team, Source);
        effects.AddEffect(new KnockbackEffect(Effects.Team, Source, 50,
                                              Rotation.FromRadians(RotationZ + MathConstants.HalfTurn),
                                              KnockbackBehavior.Additive));
        // Holder.Effects.Handle(effects);

        GunModel.CurrentRoundsInMagazine--;

        Saiga12SingleShot1mSide.Play(0.1f, 0, 0);
    }
}