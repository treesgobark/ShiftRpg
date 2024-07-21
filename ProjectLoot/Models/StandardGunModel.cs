using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using Microsoft.Xna.Framework.Audio;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Models;

public partial class StandardGunModel : IGunModel
{
    private StateMachine States { get; } = new();

    public StandardGunModel(GunData           gunData, IGunComponent gunComponent, IGunViewModel gunViewModel,
                            IEffectsComponent holderEffects)
    {
        GunData                 = gunData;
        GunComponent            = gunComponent;
        GunViewModel            = gunViewModel;
        HolderEffects           = holderEffects;
        CurrentRoundsInMagazine = gunData.MagazineSize;

        States.Add(new NotEquipped(States, FrbTimeManager.Instance, this));
        States.Add(new Ready(States, FrbTimeManager.Instance, this));
        States.Add(new Recovery(States, FrbTimeManager.Instance, this));
        States.Add(new Reloading(States, FrbTimeManager.Instance, this));
        States.InitializeStartingState<NotEquipped>();

        GunshotSound = gunData.GunName switch
        {
            GunData.Rifle   => GlobalContent.Saiga12SingleShot1mSide,
            GunData.Shotgun => GlobalContent.ShotgunBlast,
            GunData.Pistol  => GlobalContent.ThudShot,
            _               => throw new ArgumentException($"Unrecognized gun name: {gunData.GunName}")
        };
    }

    public SoundEffect GunshotSound { get; }
    public GunData GunData { get; }
    private IGunComponent GunComponent { get; }
    private IGunViewModel GunViewModel { get; }
    private IEffectsComponent HolderEffects { get; }

    private IEffectBundle OnFireHolderEffects
    {
        get
        {
            var effects = new EffectBundle();
            effects.AddEffect(new KnockbackEffect(GunComponent.Team, SourceTag.Gun, 50,
                                                  GunComponent.GunRotation + Rotation.HalfTurn,
                                                  KnockbackBehavior.Additive));
            return effects;
        }
    }

    private IEffectBundle OnHitHolderEffects => new EffectBundle();

    private IEffectBundle OnHitTargetEffects
    {
        get
        {
            var effects = new EffectBundle();
            effects.AddEffect(new DamageEffect(~GunComponent.Team, SourceTag.Gun, GunData.Damage));
            effects.AddEffect(new HitstopEffect(GunComponent.Team, SourceTag.Gun, TimeSpan.FromMilliseconds(GunData.HitstopDurationMilliseconds)));
            effects.AddEffect(new KnockbackEffect(~GunComponent.Team, SourceTag.Gun, GunData.KnockbackVelocity,
                                                  GunComponent.GunRotation, KnockbackBehavior.Replacement));
            // effects.AddEffect(new ShatterDamageEffect(~GunComponent.Team, SourceTag.Gun, GunData.Damage));
            return effects;
        }
    }

    public int CurrentRoundsInMagazine { get; set; }
    public bool IsEquipped { get; set; } = false;
    public bool IsFull => CurrentRoundsInMagazine  == GunData.MagazineSize;
    public bool IsEmpty => CurrentRoundsInMagazine == 0;

    public void Activity()
    {
        States.DoCurrentStateActivity();
    }
}