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
    private class Slash3 : ParentedTimedState<SwordModel>
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
        
        public Slash3(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel parent)
            : base(stateMachine, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
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
                Radius                  = 6,
                RelativeX               = 18,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };

            hitboxShape.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape);

            Hitbox.SpriteInstance.CurrentChainName  = "Slash1";
            Hitbox.SpriteInstance.AnimationSpeed    = 0.99f / (float)Duration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeRotationZ = AttackDirection.NormalizedRadians;
            Hitbox.SpriteInstance.RelativeZ         = 0.1f;
            
            // Parent.HolderEffects.Handle(
            //     new KnockbackEffect(
            //         Parent.MeleeWeaponComponent.Team,
            //         SourceTag.None,
            //         200,
            //         AttackDirection,
            //         KnockbackBehavior.Replacement
            //     )
            // );
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                NextState = StateMachine.Get<CircleSlash>();
            }

            if (TimeInState >= Duration)
            {
                if (!Parent.IsEquipped)
                {
                    return StateMachine.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }
                
                return StateMachine.Get<Slash3Recovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Hitbox != null)
            {
                Hitbox.SpriteInstance.AnimateSelf(0);

                Hitbox.RelativeRotationZ =
                    (HitboxStartDirection + Rotation.HalfTurn * NormalizedProgress).NormalizedRadians;

                if (SegmentsHandled < GoalSegmentsHandled)
                {
                    EffectBundle targetHitEffects = new();
            
                    targetHitEffects.AddEffect(new DamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Melee, 5));
                    
                    targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Melee,
                                                                 HitstopDuration));

                    targetHitEffects.AddEffect(
                        new KnockbackEffect(
                            ~Parent.MeleeWeaponComponent.Team,
                            SourceTag.Melee,
                            200,
                            AttackDirection,
                            KnockbackBehavior.Replacement
                        )
                    );
            
                    Hitbox.TargetHitEffects = targetHitEffects;
                    
                    EffectBundle holderHitEffects = new();
            
                    holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Melee, HitstopDuration));
            
                    Hitbox.HolderHitEffects = holderHitEffects;

                    SegmentsHandled++;
                }
            }
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}