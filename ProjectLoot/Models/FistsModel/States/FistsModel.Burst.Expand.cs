using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;

namespace ProjectLoot.Models;

public partial class FistsModel
{
    private class BurstExpand : ModularDelegateState
    {
        public BurstExpand(IReadonlyStateMachine states, IMeleeWeaponModel model, ITimeManager timeManager, Burst burst)
        {
            DurationModule duration = AddModule(new DurationModule(FrbTimeManager.Instance, TimeSpan.FromMilliseconds(60)));
            AddActivate(() =>
            {
                burst.Hitbox = MeleeHitbox.CreateHitbox(model)
                                          .AddCircle(36)
                                          .AddSpriteInfo("Shockwave")
                                          .Build();
                var bundle = new EffectBundle();
                bundle.AddEffect(new AttackEffect(~model.MeleeWeaponComponent.Team, SourceTag.Fists, 25));
                bundle.AddEffect(new KnockTowardEffect
                {
                    AppliesTo = ~model.MeleeWeaponComponent.Team,
                    Source = SourceTag.Fists,
                    TargetPosition = model.MeleeWeaponComponent.HolderGameplayCenterPosition,
                    Strength = -800,
                });
                bundle.AddEffect(new HitstopEffect(~model.MeleeWeaponComponent.Team, SourceTag.Fists, TimeSpan.FromMilliseconds(250)));
                burst.Hitbox.TargetHitEffects = bundle;

                var holderBundle = new EffectBundle();
                holderBundle.AddEffect(new HitstopEffect(model.MeleeWeaponComponent.Team, SourceTag.Fists, TimeSpan.FromMilliseconds(150)));
                burst.Hitbox.HolderHitEffects = holderBundle;
                
                GlobalContent.Shockwave.Play(0.3f, 0, 0);
            });
            AddUpdate(() => burst.Hitbox.SpriteInstance.TextureScale = duration.NormalizedProgress);
            AddModule(new DurationExitModule<BurstFade>(duration, states));
        }
    }
}
