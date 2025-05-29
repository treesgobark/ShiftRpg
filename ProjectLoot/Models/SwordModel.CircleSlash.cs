using ANLG.Utilities.Core.Extensions;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class CircleSlash : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(240);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(50);
        private static TimeSpan FinalHitstopDuration => TimeSpan.FromMilliseconds(250);
        private float NormalizedProgress => (float)(TimeInState / Duration);

        private MeleeHitbox? Hitbox { get; set; }
        private Rotation AttackDirection { get; set; }
        private Rotation HitboxStartDirection => AttackDirection + Rotation.QuarterTurn;

        private static int TotalSegments => 2;
        private static int TotalWhooshes => 3;
        private int SegmentsHandled { get; set; }
        private int WhooshesHandled { get; set; }
        private int GoalSegmentsHandled => Math.Clamp((int)(NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);
        private int GoalWhooshesHandled => Math.Clamp((int)(NormalizedProgress * TotalWhooshes) + 1, 0, TotalWhooshes);
        private bool IsFinalSegment => SegmentsHandled >= TotalSegments - 1;
        private bool _hasMadeContact;
        private IState? NextState { get; set; }
        
        public CircleSlash(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            SegmentsHandled = 0;
            WhooshesHandled = 0;

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
            
            Parent.HolderEffects.Handle(
                new KnockbackEffect(
                    Parent.MeleeWeaponComponent.Team,
                    SourceTag.None,
                    200,
                    AttackDirection,
                    KnockbackBehavior.Additive
                )
            );
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState >= Duration)
            {
                if (!Parent.IsEquipped)
                {
                    return States.Get<NotEquipped>();
                }

                return States.Get<CircleSlashRecovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.RelativeRotationZ =
                (HitboxStartDirection - (Rotation.FullTurn + Rotation.HalfTurn) * NormalizedProgress).NormalizedRadians;
            Hitbox.SpriteInstance.Alpha = MathF.Sqrt(1f - NormalizedProgress);

            if (SegmentsHandled < GoalSegmentsHandled)
            {
                EffectBundle targetHitEffects = new();
        
                targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, IsFinalSegment ? 25 : 12));
                
                targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                             IsFinalSegment ? FinalHitstopDuration : HitstopDuration));

                targetHitEffects.AddEffect(
                    new KnockbackEffect(
                        ~Parent.MeleeWeaponComponent.Team,
                        SourceTag.Sword,
                        400             + 800                * SegmentsHandled,
                        AttackDirection - Rotation.EighthTurn / 2,
                        KnockbackBehavior.Replacement
                        )
                    );
                
                targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
        
                Hitbox.TargetHitEffects = targetHitEffects;
                
                EffectBundle holderHitEffects = new();
        
                holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, IsFinalSegment ? FinalHitstopDuration : HitstopDuration));
        
                Hitbox.HolderHitEffects = holderHitEffects;
                
                float pitch = Random.Shared.NextSingle(-0.1f, 0.1f);
                GlobalContent.BladeSwingF.Play(0.15f, pitch, 0);

                SegmentsHandled++;
            }

            if (WhooshesHandled < GoalWhooshesHandled)
            {
                float pitch = Random.Shared.NextSingle(-0.1f, 0.1f);
                GlobalContent.WhooshB.Play(0.2f, pitch, 0);

                WhooshesHandled++;
            }
        }

        public override void BeforeDeactivate(IState? nextState)
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}