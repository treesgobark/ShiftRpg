using System.Collections.Generic;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossActive : ParentedTimedState<SpearModel>
    {
        private EffectBundle _targetHitEffects;
        private static TimeSpan ActiveDuration => TimeSpan.FromMilliseconds(120);
        
        private float NormalizedProgress => (float)(TimeInState / ActiveDuration).Saturate();
        
        private bool HasAddedHitEffects { get; set; }
        private static float MinTravelDistance => 24;
        private static float MaxTravelDistance => 196;
        private static float MinDamage => 10;
        private static float MaxDamage => 20;
        private static float MinPoiseDamage => 10;
        private static float MaxPoiseDamage => 30;
        private static float MinKnockbackVelocity => 400;
        private static float MaxKnockbackVelocity => 1200;
        private static float DistanceDamageMultiplier => 4f;
        private static TimeSpan MinHitstopDuration => TimeSpan.FromMilliseconds(50);
        private static TimeSpan MaxHitstopDuration => TimeSpan.FromMilliseconds(250);

        private Vector3 CurrentTravelVector => _initialHitboxPosition +
                                               Vector3Extensions.FromRotationAndLength(
                                                   Parent.AttackDirection, NormalizedProgress * TravelDistance);
        
        private float DamageFromCharge => MathHelper.Lerp(MinDamage,             MaxDamage,            Parent.ChargeProgress);

        private float Damage =>
            MathHelper.Lerp(DamageFromCharge, DamageFromCharge * DistanceDamageMultiplier, NormalizedProgress * NormalizedProgress);
        private float PoiseDamage => MathHelper.Lerp(MinPoiseDamage,             MaxPoiseDamage,       Parent.ChargeProgress);
        private float KnockbackVelocity => MathHelper.Lerp(MinKnockbackVelocity, MaxKnockbackVelocity, Parent.ChargeProgress);
        private TimeSpan HitstopDuration => MathUtilities.Lerp(MinHitstopDuration, MaxHitstopDuration, Parent.ChargeProgress);
        private float TravelDistance => MathHelper.Lerp(MinTravelDistance,       MaxTravelDistance,    Parent.ChargeProgress);

        private Vector3 _initialHitboxPosition;

        public TossActive(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Hitbox.AttachTo(null);
            AddHitEffects();
            HasAddedHitEffects = true;
            GlobalContent.SwingA.Play(0.1f, 0, 0);
            Parent.Hitbox.IsActive = true;
            _initialHitboxPosition = Parent.Hitbox.Position;
        }

        public override IState? EvaluateExitConditions()
        {
            if (NormalizedProgress >= 1)
            {
                if (!Parent.IsEquipped)
                {
                    Parent.Hitbox?.Destroy();
                    return States.Get<NotEquipped>();
                }

                return States.Get<TossedSpearInGround>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.X = CurrentTravelVector.X;
            Parent.Hitbox.Y = CurrentTravelVector.Y;
            UpdateHitEffects();
        }

        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize() { }

        private void UpdateHitEffects()
        {
            _targetHitEffects.UpsertEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, Damage));
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
                    Parent.AttackDirection,
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
