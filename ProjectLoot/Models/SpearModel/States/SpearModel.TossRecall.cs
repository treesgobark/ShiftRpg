using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossRecall : ParentedTimedState<SpearModel>
    {
        private EffectBundle _targetHitEffects;

        private float MinDamage => 15;
        private float MaxDamage => 30;
        private float Damage => float.Lerp(MaxDamage, MinDamage, NormalizedProgress);
        private float PoiseDamage => 10;
        private float KnockbackVelocity => 400;
        private float MinLerpCoefficient => 0.05f;
        private float MaxLerpCoefficient => 0.2f;
        private float LerpCoefficient => float.Lerp(MinLerpCoefficient, MaxLerpCoefficient, NormalizedProgress);
        private TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(100);
        private float CompletionDistance => 16f;

        private float NormalizedProgress => 1f - (DistanceFromGameplayCenter /
                                             (DistanceFromInitialPosition + DistanceFromGameplayCenter)).Saturate();
        private float DistanceFromInitialPosition => Vector3.Distance(Parent.Hitbox.Position, _initialHitboxPosition);
        private float DistanceFromGameplayCenter =>
            Vector3.Distance(Parent.Hitbox.Position, Parent.MeleeWeaponComponent.HolderGameplayCenterPosition);
        private Vector3 VectorToGameplayCenter =>
            Parent.Hitbox.Position.GetVectorTo(Parent.MeleeWeaponComponent.HolderGameplayCenterPosition);
        

        public TossRecall(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        private Vector3 _initialHitboxPosition;
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            AddHitEffects();
            GlobalContent.SwingA.Play(0.1f, 0, 0);
            Parent.Hitbox.IsActive = true;
            _initialHitboxPosition = Parent.Hitbox.Position;
        }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }
            
            if (DistanceFromGameplayCenter <= CompletionDistance)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.RotationZ = VectorToGameplayCenter.AngleOrZero();
            Parent.Hitbox.Position = Vector3.Lerp(Parent.Hitbox.Position,
                                                  Parent.MeleeWeaponComponent.HolderGameplayCenterPosition,
                                                  LerpCoefficient);
            Parent.Hitbox.SpriteInstance.Alpha = 1f - NormalizedProgress * NormalizedProgress;
            UpdateHitEffects();
        }

        public override void BeforeDeactivate()
        {
            Parent.Hitbox?.Destroy();
        }

        public override void Uninitialize() { }

        private void UpdateHitEffects()
        {
            _targetHitEffects.UpsertEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, Damage));
            
            _targetHitEffects.UpsertEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Spear,
                    KnockbackVelocity,
                    Rotation.FromRadians(VectorToGameplayCenter.AngleOrZero()),
                    KnockbackBehavior.Replacement
                )
            );
        }

        private void AddHitEffects()
        {
            _targetHitEffects = new();
            
            _targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, Damage));
                    
            _targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear,
                                                         HitstopDuration));

            _targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Spear,
                    KnockbackVelocity,
                    Rotation.FromRadians(VectorToGameplayCenter.AngleOrZero()),
                    KnockbackBehavior.Replacement
                )
            );
                
            _targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, PoiseDamage));
            
            Parent.Hitbox.TargetHitEffects = _targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Spear, HitstopDuration));
        
            Parent.Hitbox.HolderHitEffects = holderHitEffects;
        }
    }
}
