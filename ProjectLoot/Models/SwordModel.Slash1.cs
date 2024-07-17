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
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(180);
        private float NormalizedProgress => (float)(TimeInState / Duration);

        private MeleeHitbox? Hitbox { get; set; }
        private Rotation AttackDirection { get; set; }
        private Rotation HitboxStartDirection => AttackDirection - Rotation.QuarterTurn;
        
        private int TotalSegments => 3;
        private int SegmentsHandled { get; set; }
        private int GoalSegmentsHandled => Math.Clamp((int)(NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);
        
        private IState? NextState { get; set; }
        
        public Slash1(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel parent)
            : base(stateMachine, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            SegmentsHandled = 0;
            
            NextState = null;
            
            Hitbox = MeleeHitboxFactory.CreateNew();
            Hitbox.AttachTo(Parent.MeleeWeaponComponent.Holder);
            Hitbox.ParentRotationChangesPosition = false;
            Hitbox.ParentRotationChangesRotation = false;

            Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
            
            Circle hitboxShape = new()
            {
                Radius    = 6,
                RelativeX = 18,
                Visible   = true,
                IgnoresParentVisibility = true,
            };

            hitboxShape.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape);

            AttackDirection = Rotation.FromRadians(Parent.MeleeWeaponComponent.Holder.RotationZ);
            
            Parent.HolderEffects.Handle(
                new KnockbackEffect(
                    Parent.MeleeWeaponComponent.Team,
                    SourceTag.None,
                    200,
                    AttackDirection,
                    KnockbackBehavior.Replacement
                    )
                );
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                NextState = StateMachine.Get<Slash2>();
            }
            
            if (TimeInState >= Duration)
            {
                if (NextState is not null)
                {
                    return NextState;
                }

                return StateMachine.Get<Slash1Recovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Hitbox != null)
            {
                Hitbox.RelativeRotationZ =
                    (HitboxStartDirection + Rotation.HalfTurn * NormalizedProgress).NormalizedRadians;

                if (SegmentsHandled < GoalSegmentsHandled)
                {
                    EffectBundle targetHitEffects = new();
            
                    targetHitEffects.AddEffect(new DamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Melee, 4));
                    
                    // targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Melee,
                    //                                              TimeSpan.FromMilliseconds(100)));

                    targetHitEffects.AddEffect(
                        new KnockbackEffect(
                            ~Parent.MeleeWeaponComponent.Team, SourceTag.Melee,
                            100,
                            Rotation.FromRadians(Parent.MeleeWeaponComponent.Holder.RotationZ),
                            KnockbackBehavior.Replacement
                        )
                    );
            
                    Hitbox.TargetHitEffects = targetHitEffects;
                    
                    EffectBundle holderHitEffects = new();
            
                    // holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Melee, TimeSpan.FromMilliseconds(100)));
            
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