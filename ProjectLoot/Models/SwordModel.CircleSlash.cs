using ANLG.Utilities.Core.Extensions;
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
    private class CircleSlash : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(240);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(100);
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
        
        private IState? NextState { get; set; }
        
        public CircleSlash(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
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
                Radius                  = 6,
                RelativeX               = 18,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };

            hitboxShape.AttachTo(Hitbox);
            Hitbox.Collision.Add(hitboxShape);

            Hitbox.SpriteInstance.CurrentChainName = "ThreeEighthsSlash";
            Hitbox.SpriteInstance.AnimationSpeed   = 0.99f / (float)Duration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeZ        = 0.2f;
            Hitbox.SpriteInstance.FlipVertical     = true;
            
            Parent.HolderEffects.Handle(
                new KnockbackEffect(
                    Parent.MeleeWeaponComponent.Team,
                    SourceTag.None,
                    100,
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

            if (SegmentsHandled < GoalSegmentsHandled)
            {
                EffectBundle targetHitEffects = new();
        
                targetHitEffects.AddEffect(new DamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 12));
                
                targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                             HitstopDuration));

                targetHitEffects.AddEffect(
                    new KnockbackEffect(
                        ~Parent.MeleeWeaponComponent.Team,
                        SourceTag.Sword,
                        200 + 300 * NormalizedProgress,
                        AttackDirection,
                        KnockbackBehavior.Replacement
                        )
                    );
        
                Hitbox.TargetHitEffects = targetHitEffects;
                
                EffectBundle holderHitEffects = new();
        
                holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
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

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}