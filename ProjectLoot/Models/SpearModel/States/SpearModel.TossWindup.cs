using System.Collections.Generic;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Debugging;
using FlatRedBall.Math.Geometry;
using GlueControl.Editing;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossWindup : ParentedTimedState<SpearModel>
    {
        private static TimeSpan MaxChargeDuration => TimeSpan.FromMilliseconds(480);
        private static TimeSpan MaxWindupDuration => TimeSpan.FromMilliseconds(960);

        private float NormalizedProgress => (float)(TimeInState / MaxWindupDuration).Saturate();
        private float ChargeProgress => (float)(TimeInState / MaxChargeDuration).Saturate();
        
        private bool IsCharged => ChargeProgress >= 1;

        private static float LateralOffset => 24;
        private static float PerpendicularOffset => -4;
        private static float WindupDistance => -16;
        private static float HitboxSpriteOffset => -24;

        private Vector3 CurrentWindupVector =>
            Vector3Extensions.FromRotationAndLength(Parent.AttackDirection, LateralOffset + ChargeProgress * WindupDistance)
            + Vector3Extensions.FromRotationAndLength(Parent.AttackDirection + Rotation.QuarterTurn, PerpendicularOffset);
        
        private static float CircleRadius => 4;
        private static int CircleCount => 5;
        private static float TotalCircleLength => 48f;
        private static float DistanceBetweenCircles =>
            CircleCount > 1 ? TotalCircleLength / (CircleCount - 1) : TotalCircleLength;
        
        private float ZOffset { get; set; }

        public TossWindup(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            Parent.AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

            CalculateZOffset();
            CreateHitbox();
            AddHitboxCollision();
            ConfigureHitboxSprite();
        }

        public override IState? EvaluateExitConditions()
        {
            if (NormalizedProgress >= 1)
            {
                return States.Get<Idle>();
            }
            
            if (!Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown)
            {
                if (!IsCharged)
                {
                    return States.Get<Idle>();
                }
                else
                {
                    return States.Get<TossActive>();
                }
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.RelativeX = CurrentWindupVector.X;
            Parent.Hitbox.RelativeY = CurrentWindupVector.Y;
        }

        public override void BeforeDeactivate(IState? nextState)
        {
            if (nextState is TossActive)
            {
                Parent.ChargeProgress = ChargeProgress;
                return;
            }
            
            Parent.Hitbox?.Destroy();
        }

        public override void Uninitialize() { }

        private void CalculateZOffset()
        {
            int sector = Parent.AttackDirection.GetSector(8, true);
            ZOffset = sector switch { 6 or 7 or 0 or 1 => 0.2f, 2 or 3 or 4 or 5 => -0.2f, _ => ZOffset };
        }
        
        private void CreateHitbox()
        {
            Parent.Hitbox = MeleeHitboxFactory.CreateNew();
            Parent.MeleeWeaponComponent.AttachObjectToAttackOrigin(Parent.Hitbox);
            Parent.Hitbox.ParentRotationChangesPosition = false;
            Parent.Hitbox.ParentRotationChangesRotation = false;
            Parent.Hitbox.RelativeRotationZ             = Parent.AttackDirection.NormalizedRadians;
            Parent.Hitbox.IsActive                      = false;

            Parent.Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Parent.Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
        }

        private void AddHitboxCollision()
        {
            for (int i = 0; i < CircleCount; i++)
            {
                var circle = new Circle
                {
                    Radius                  = CircleRadius,
                    Visible                 = false,
                    IgnoresParentVisibility = true,
                    RelativeX               = -i * DistanceBetweenCircles,
                    Color                   = i == 0 ? Color.Red : Color.White,
                };

                circle.AttachTo(Parent.Hitbox);
                Parent.Hitbox.Collision.Add(circle);
            }
        }

        private void ConfigureHitboxSprite()
        {
            Parent.Hitbox.SpriteInstance.CurrentChainName = "SpearThrust";
            Parent.Hitbox.SpriteInstance.RelativeX        = HitboxSpriteOffset;

            Parent.Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
