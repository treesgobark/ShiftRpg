using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class Slash2 : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(120);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(50);
        private float NormalizedProgress => (float)(TimeInState / Duration);

        private MeleeHitbox? Hitbox { get; set; }
        private Rotation AttackDirection { get; set; }
        private Rotation HitboxStartDirection => AttackDirection + Rotation.QuarterTurn;

        private static int TotalSegments => 1;
        private int SegmentsHandled { get; set; }
        private int GoalSegmentsHandled => Math.Clamp((int)(NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);
        
        private IState? NextState { get; set; }
        
        public Slash2(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            SegmentsHandled = 0;
            
            NextState       = null;

            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;
            
            Hitbox = MeleeHitboxFactory.CreateNew();
            Parent.MeleeWeaponComponent.AttachObjectToAttackOrigin(Hitbox);
            Hitbox.ParentRotationChangesPosition = false;
            Hitbox.ParentRotationChangesRotation = false;

            Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
            
            Circle hitboxShape = new()
            {
                Radius                  = 10f,
                RelativeX               = 20f,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };

            hitboxShape.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape);
            
            Circle hitboxShape2 = new()
            {
                Radius                  = 2,
                RelativeX               = 8,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };

            hitboxShape2.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape2);

            Hitbox.SpriteInstance.CurrentChainName = "ThreeEighthsSlash";
            Hitbox.SpriteInstance.AnimationSpeed   = 0.99f / (float)Duration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeZ        = 0.2f;
            Hitbox.SpriteInstance.FlipVertical     = true;
            
            // Parent.HolderEffects.Handle(
            //     new KnockbackEffect(
            //         Parent.MeleeWeaponComponent.Team,
            //         SourceTag.None,
            //         200,
            //         AttackDirection,
            //         KnockbackBehavior.Replacement
            //     )
            // );

            GlobalContent.BladeSwingB.Play(0.2f, 0, 0);
            GlobalContent.WhooshA.Play(0.2f, 0, 0);
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                NextState = States.Get<Slash3>();
            }

            if (TimeInState >= Duration)
            {
                if (!Parent.IsEquipped)
                {
                    return States.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }
                
                return States.Get<Slash2Recovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.SpriteInstance.AnimateSelf(0);

            Hitbox.RelativeRotationZ =
                (HitboxStartDirection - Rotation.HalfTurn * NormalizedProgress).NormalizedRadians;
            Hitbox.SpriteInstance.Alpha = 1f - NormalizedProgress;

            if (SegmentsHandled < GoalSegmentsHandled)
            {
                EffectBundle targetHitEffects = new();
        
                targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
                
                targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                             HitstopDuration));

                targetHitEffects.AddEffect(
                    new KnockbackEffect(
                        ~Parent.MeleeWeaponComponent.Team,
                        SourceTag.Sword,
                        450,
                        AttackDirection - Rotation.EighthTurn / 2,
                        KnockbackBehavior.Replacement
                    )
                );
                
                targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
        
                Hitbox.TargetHitEffects = targetHitEffects;
                
                EffectBundle holderHitEffects = new();
        
                holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
                Hitbox.HolderHitEffects = holderHitEffects;

                SegmentsHandled++;
            }
        }

        public override void BeforeDeactivate(IState? nextState)
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}