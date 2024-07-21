using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class Slash1 : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(120);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(50);
        private float NormalizedProgress => (float)(TimeInState / Duration);

        private MeleeHitbox? Hitbox { get; set; }
        private Rotation AttackDirection { get; set; }
        private Rotation HitboxStartDirection => AttackDirection - Rotation.QuarterTurn;
        
        private static int TotalSegments => 1;
        private int SegmentsHandled { get; set; }
        private int GoalSegmentsHandled => Math.Clamp((int)(NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);
        
        private IState? NextState { get; set; }
        
        public Slash1(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            SegmentsHandled = 0;
            
            NextState = null;

            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;
            
            Hitbox = MeleeHitboxFactory.CreateNew();
            Parent.MeleeWeaponComponent.AttachObjectToAttackOrigin(Hitbox);
            Hitbox.ParentRotationChangesPosition = false;
            Hitbox.ParentRotationChangesRotation = false;

            Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
            
            Circle hitboxShape = new()
            {
                Radius    = 8,
                RelativeX = 16,
                Visible   = false,
                IgnoresParentVisibility = true,
            };

            hitboxShape.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape);

            Hitbox.SpriteInstance.CurrentChainName  = "ThreeEighthsSlash";
            Hitbox.SpriteInstance.AnimationSpeed    = 0.99f / (float)Duration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeZ         = 0.2f;

            // Parent.HolderEffects.Handle(
            //     new KnockbackEffect(
            //         Parent.MeleeWeaponComponent.Team,
            //         SourceTag.Sword,
            //         200,
            //         AttackDirection,
            //         KnockbackBehavior.Replacement
            //         )
            //     );

            GlobalContent.BladeSwingA.Play(0.1f, 0, 0);
            GlobalContent.WhooshA.Play(0.2f, 0, 0);
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                NextState = States.Get<Slash2>();
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

                return States.Get<Slash1Recovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.RelativeRotationZ =
                (HitboxStartDirection + Rotation.HalfTurn * NormalizedProgress).NormalizedRadians;

            if (SegmentsHandled < GoalSegmentsHandled)
            {
                EffectBundle targetHitEffects = new();
        
                targetHitEffects.AddEffect(new DamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
                
                targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                             HitstopDuration));

                targetHitEffects.AddEffect(
                    new KnockbackEffect(
                        ~Parent.MeleeWeaponComponent.Team,
                        SourceTag.Sword,
                        200,
                        AttackDirection,
                        KnockbackBehavior.Replacement
                    )
                );
        
                Hitbox.TargetHitEffects = targetHitEffects;
                
                EffectBundle holderHitEffects = new();
        
                holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
                Hitbox.HolderHitEffects = holderHitEffects;

                SegmentsHandled++;
            }
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}