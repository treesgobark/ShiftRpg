using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossActive : ParentedTimedState<Toss>
    {
        private readonly IReadonlyStateMachine _states;
        private EffectBundle _targetHitEffects;
        private static TimeSpan ActiveDuration => TimeSpan.FromMilliseconds(120);
        
        private float NormalizedProgress => (float)(TimeInState / ActiveDuration).Saturate();
        
        private static float MinTravelDistance => 24;
        private static float MaxTravelDistance => 196;
        private static float MinDamage => 10;
        private static float MaxDamage => 20;
        private static float MinPoiseDamage => 10;
        private static float MaxPoiseDamage => 30;
        private static float MinKnockbackVelocity => 200;
        private static float MaxKnockbackVelocity => 400;
        private static float DistanceDamageMultiplier => 2f;
        private static TimeSpan MinHitstopDuration => TimeSpan.FromMilliseconds(50);
        private static TimeSpan MaxHitstopDuration => TimeSpan.FromMilliseconds(150);

        private Vector3 CurrentTravelVector => _initialHitboxPosition +
                                               Vector3Extensions.FromRotationAndLength(
                                                   Parent.AttackDirection, NormalizedProgress * TravelDistance);
        
        private float DamageFromCharge => MathHelper.Lerp(MinDamage,             MaxDamage,            Parent.ChargeProgress);

        private float Damage =>
            MathHelper.Lerp(DamageFromCharge, DamageFromCharge * DistanceDamageMultiplier, NormalizedProgress);
        private float PoiseDamage => MathHelper.Lerp(MinPoiseDamage,             MaxPoiseDamage,       Parent.ChargeProgress);
        private float KnockbackVelocity => MathHelper.Lerp(MinKnockbackVelocity, MaxKnockbackVelocity, Parent.ChargeProgress);
        private TimeSpan HitstopDuration => MathUtilities.Lerp(MinHitstopDuration, MaxHitstopDuration, Parent.ChargeProgress);
        private float TravelDistance => MathHelper.Lerp(MinTravelDistance,       MaxTravelDistance,    Parent.ChargeProgress);

        private Vector3 _initialHitboxPosition;
        private IState? _nextState;

        public TossActive(IReadonlyStateMachine states, ITimeManager timeManager, Toss tossState)
            : base(timeManager, tossState)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
            _nextState = null;
            Parent.Hitbox.AttachTo(null);
            Parent.Hitbox.RotationX = 0;
            Parent.Hitbox.RotationY = 0;
            AddHitEffects();
            GlobalContent.SwingA.Play(0.1f, 0, 0);
            Parent.Hitbox.IsActive = true;
            _initialHitboxPosition = Parent.Hitbox.Position;
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                _nextState = _states.Get<TossRecall>();
            }
            
            if (NormalizedProgress >= 1)
            {
                if (_nextState is not null)
                {
                    return _nextState;
                }

                return _states.Get<TossedSpearInGround>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.X = CurrentTravelVector.X;
            Parent.Hitbox.Y = CurrentTravelVector.Y;
            UpdateHitEffects();
        }

        public override void BeforeDeactivate() { }

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
