using System.Collections.Generic;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
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
    private class Toss : ParentedTimedState<SpearModel>
    {
        private EffectBundle _targetHitEffects;
        private static TimeSpan MinStartupDuration => TimeSpan.FromMilliseconds(180);
        private static TimeSpan MaxStartupDuration => TimeSpan.FromMilliseconds(480);
        private static TimeSpan ActiveDuration => TimeSpan.FromMilliseconds(120);
        private static TimeSpan RecoveryDuration => TimeSpan.FromMilliseconds(120);
        
        private float NormalizedProgress { get; set; }
        private float ChargeProgress { get; set; }
        
        private float StartupProgress => NormalizedProgress  * 3;
        private float ActiveProgress => NormalizedProgress   * 3 - 1;
        private float RecoveryProgress => NormalizedProgress * 3 - 2;

        private float MinStartupProgressPerFrame => (float)(TimeManager.GameTimeSinceLastFrame / MinStartupDuration / 3);
        private float MaxStartupProgressPerFrame => (float)(TimeManager.GameTimeSinceLastFrame / MaxStartupDuration / 3);
        private float ActiveProgressPerFrame => (float)(TimeManager.GameTimeSinceLastFrame     / ActiveDuration / 3);
        private float RecoveryProgressPerFrame => (float)(TimeManager.GameTimeSinceLastFrame   / RecoveryDuration / 3);
        private float ChargeProgressPerFrame => (float)(TimeManager.GameTimeSinceLastFrame / MaxStartupDuration);

        private bool IsStartingUp => StartupProgress is >= 0 and < 1;
        private bool IsActive => ActiveProgress is >= 0 and < 1;
        private bool IsRecovering => RecoveryProgress is >= 0 and < 1;
        
        private bool HasAddedHitEffects { get; set; }
        private bool HasRemovedHitEffects { get; set; }
        
        private static float HitboxRadius => 4;
        private static float PerpendicularOffset => -4;
        private static float WindupDistance => -16;
        private static float MinTravelDistance => 24;
        private static float MaxTravelDistance => 196;
        private static float InitialDistance => 24;
        private static float HitboxSpriteOffset => -24;
        private static float MinDamage => 10;
        private static float MaxDamage => 20;
        private static float MinPoiseDamage => 10;
        private static float MaxPoiseDamage => 30;
        private static float MinKnockbackVelocity => 400;
        private static float MaxKnockbackVelocity => 1200;
        private static float DistanceDamageMultiplier => 4f;
        private static TimeSpan MinHitstopDuration => TimeSpan.FromMilliseconds(50);
        private static TimeSpan MaxHitstopDuration => TimeSpan.FromMilliseconds(250);
        
        private float DamageFromCharge => MathHelper.Lerp(MinDamage,             MaxDamage,            ChargeProgress);

        private float Damage =>
            MathHelper.Lerp(DamageFromCharge, DamageFromCharge * DistanceDamageMultiplier, ActiveProgress * ActiveProgress);
        private float PoiseDamage => MathHelper.Lerp(MinPoiseDamage,             MaxPoiseDamage,       ChargeProgress);
        private float KnockbackVelocity => MathHelper.Lerp(MinKnockbackVelocity, MaxKnockbackVelocity, ChargeProgress);
        private TimeSpan HitstopDuration => MathUtilities.Lerp(MinHitstopDuration, MaxHitstopDuration, ChargeProgress);
        private float TravelDistance => MathHelper.Lerp(MinTravelDistance,       MaxTravelDistance,    ChargeProgress);

        private static int CircleCount => 5;
        private static float TotalCircleLength => 48f;
        private static float DistanceBetweenCircles =>
            CircleCount > 1 ? TotalCircleLength / (CircleCount - 1) : DistanceBetweenCircles;
        
        private MeleeHitbox? Hitbox { get; set; }
        private List<Circle> Circles { get; set; }
        private Rotation AttackDirection { get; set; }
        private float ZOffset { get; set; }
        
        private IState? NextState { get; set; }

        public Toss(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            NextState          = null;
            NormalizedProgress = 0;
            ChargeProgress     = 0;
            HasAddedHitEffects = false;

            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

            CalculateZOffset();
            CreateHitbox();
            AddHitboxCollision();
            ConfigureHitboxSprite();
        }

        public override IState? EvaluateExitConditions()
        {
            if (RecoveryProgress >= 1)
            {
                if (!Parent.IsEquipped)
                {
                    return States.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }

                // return States.Get<TossRecovery>();
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            if (IsRecovering)
            {
                Hitbox.IsActive    =  false;
                NormalizedProgress += RecoveryProgressPerFrame;
                Hitbox.SpriteInstance.Alpha = 1f - RecoveryProgress;
            }
            
            if (IsActive)
            {
                if (Hitbox.Parent is not null)
                {
                    Hitbox.AttachTo(null);
                }
                
                if (!HasAddedHitEffects)
                {
                    AddTargetHitEffects();
                    HasAddedHitEffects = true;
                    GlobalContent.SwingA.Play(0.1f, 0, 0);
                }
                else
                {
                    UpdateHitEffects();
                }
                
                Hitbox.IsActive = true;
                Hitbox.SpriteInstance.RelativeX = InitialDistance                     + WindupDistance +
                                                  ActiveProgress * TravelDistance + HitboxSpriteOffset;
                
                for (int i = 0; i < CircleCount; i++)
                {

                    Circles[i].RelativeX = InitialDistance + WindupDistance + ActiveProgress * TravelDistance -
                                           i                                                 * DistanceBetweenCircles;
                }

                NormalizedProgress          += ActiveProgressPerFrame;
            }
            
            if (IsStartingUp)
            {
                Hitbox.SpriteInstance.RelativeX = InitialDistance + StartupProgress * WindupDistance + HitboxSpriteOffset;

                for (int i = 0; i < CircleCount; i++)
                {
                    Circles[i].RelativeX = InitialDistance + StartupProgress * WindupDistance -
                                           i                                 * DistanceBetweenCircles;
                }
                NormalizedProgress += Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown
                    ? MaxStartupProgressPerFrame
                    : MinStartupProgressPerFrame;

                if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown)
                {
                    ChargeProgress += ChargeProgressPerFrame;
                }
            }
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }

        private void CalculateZOffset()
        {
            int sector = AttackDirection.GetSector(8, true);
            ZOffset = sector switch { 6 or 7 or 0 or 1 => 0.2f, 2 or 3 or 4 or 5 => -0.2f, _ => ZOffset };
        }
        
        private void CreateHitbox()
        {
            Hitbox = MeleeHitboxFactory.CreateNew();
            Parent.MeleeWeaponComponent.AttachObjectToAttackOrigin(Hitbox);
            Hitbox.ParentRotationChangesPosition = false;
            Hitbox.ParentRotationChangesRotation = false;
            Hitbox.RelativeRotationZ             = AttackDirection.NormalizedRadians;
            Hitbox.IsActive                      = false;

            Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
        }

        private void UpdateHitEffects()
        {
            _targetHitEffects.UpsertEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, Damage));
        }

        private void AddTargetHitEffects()
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
                    AttackDirection,
                    KnockbackBehavior.Replacement
                )
            );
                
            _targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, PoiseDamage));
            
            // targetHitEffects.AddEffect(new WeaknessDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, 1));
            // targetHitEffects.AddEffect(new ApplyShatterEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear));
            
            Hitbox.TargetHitEffects = _targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
            Hitbox.HolderHitEffects = holderHitEffects;
        }

        private void AddHitboxCollision()
        {
            Circles = [];
            for (int i = 0; i < CircleCount; i++)
            {
                var circle = new Circle
                {
                    Radius                  = HitboxRadius,
                    Visible                 = false,
                    IgnoresParentVisibility = true,
                    RelativeX               = InitialDistance - HitboxSpriteOffset - i * DistanceBetweenCircles,
                    RelativeY               = PerpendicularOffset,
                };

                circle.AttachTo(Hitbox);
                Hitbox.Collision.Add(circle);

                Circles.Add(circle);
            }
        }

        private void ConfigureHitboxSprite()
        {
            Hitbox.SpriteInstance.CurrentChainName = "SpearThrust";
            Hitbox.SpriteInstance.AnimationSpeed   = 0.99f / (float)ActiveDuration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeX        = InitialDistance;
            Hitbox.SpriteInstance.RelativeY        = PerpendicularOffset;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
