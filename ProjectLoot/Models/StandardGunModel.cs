using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Models;

public partial class StandardGunModel : IGunModel
{
    private StateMachine StateMachine { get; } = new();

    public StandardGunModel(GunData           gunData, IGunComponent gunComponent, IGunViewModel gunViewModel,
                            IEffectsComponent holderEffects)
    {
        GunData                 = gunData;
        GunComponent            = gunComponent;
        GunViewModel            = gunViewModel;
        HolderEffects           = holderEffects;
        CurrentRoundsInMagazine = gunData.MagazineSize;

        StateMachine.Add(new NotEquipped(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Ready(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Recovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Reloading(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.InitializeStartingState<NotEquipped>();
    }

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
            return effects;
        }
    }

    public int CurrentRoundsInMagazine { get; set; }
    public bool IsEquipped { get; set; } = false;
    public bool IsFull => CurrentRoundsInMagazine  == GunData.MagazineSize;
    public bool IsEmpty => CurrentRoundsInMagazine == 0;
    
    public void Activity()
    {
        StateMachine.DoCurrentStateActivity();
    }
}