using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class HeavyRightJab : ParentedTimedState<FistsModel>
    {
        private static TimeSpan MinStartupDuration => TimeSpan.FromMilliseconds(18);
        private static TimeSpan MaxStartupDuration => TimeSpan.FromMilliseconds(480);
        private static TimeSpan ActiveDuration => TimeSpan.FromMilliseconds(60);
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
        
        private static float HitboxRadius => 24;
        private static float PerpendicularOffset => -4;
        private static float TravelDistance => 16;
        private static float InitialDistance => 4;
        private static float MinDamage => 16;
        private static float MaxDamage => 28;
        private static float MinPoiseDamage => 10;
        private static float MaxPoiseDamage => 30;
        private static float MinKnockbackVelocity => 400;
        private static float MaxKnockbackVelocity => 1200;
        private static TimeSpan MinHitstopDuration => TimeSpan.FromMilliseconds(50);
        private static TimeSpan MaxHitstopDuration => TimeSpan.FromMilliseconds(250);
        
        private float Damage => MathHelper.Lerp(MinDamage,                       MaxDamage,            ChargeProgress);
        private float PoiseDamage => MathHelper.Lerp(MinPoiseDamage,             MaxPoiseDamage,       ChargeProgress);
        private float KnockbackVelocity => MathHelper.Lerp(MinKnockbackVelocity, MaxKnockbackVelocity, ChargeProgress);
        private TimeSpan HitstopDuration => MathUtilities.Lerp(MinHitstopDuration, MaxHitstopDuration, ChargeProgress);

        private MeleeHitbox? Hitbox { get; set; }
        private Circle? Circle { get; set; }
        private Rotation AttackDirection { get; set; }
        private float ZOffset { get; set; }
        
        private IState? NextState { get; set; }

        public HeavyRightJab(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
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
            // if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            // {
            //     NextState = States.Get<HeavyLeftJab>();
            // }
            
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

                return States.Get<HeavyRightJabRecovery>();
                // return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.SpriteInstance.Alpha = (2f - ActiveProgress - RecoveryProgress) / 2f;
            
            if (IsRecovering)
            {
                Hitbox.IsActive    =  false;
                NormalizedProgress += RecoveryProgressPerFrame;
            }
            
            if (IsActive)
            {
                if (!HasAddedHitEffects && ActiveProgress >= 0.5f)
                {
                    AddTargetHitEffects();
                    HasAddedHitEffects = true;
                    GlobalContent.SwingA.Play(0.1f, 0, 0);
                }
                
                Hitbox.IsActive = true;
                Hitbox.SpriteInstance.RelativeX = InitialDistance - TravelDistance + ActiveProgress * 2 * TravelDistance;
                Circle.RelativeX                = InitialDistance - TravelDistance + ActiveProgress * 2 * TravelDistance;

                NormalizedProgress          += ActiveProgressPerFrame;
            }
            
            if (IsStartingUp)
            {
                Hitbox.SpriteInstance.RelativeX = InitialDistance + StartupProgress * -TravelDistance;

                NormalizedProgress += Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown
                    ? MaxStartupProgressPerFrame
                    : MinStartupProgressPerFrame;

                if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown)
                {
                    ChargeProgress += ChargeProgressPerFrame;
                }
            }
        }

        public override void BeforeDeactivate(IState? nextState)
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

        private void AddTargetHitEffects()
        {
            EffectBundle targetHitEffects = new();
            
            targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists, Damage));
                    
            targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists,
                                                         HitstopDuration));

            targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Fists,
                    KnockbackVelocity,
                    AttackDirection,
                    KnockbackBehavior.Replacement
                )
            );
                
            targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, PoiseDamage));
            
            // targetHitEffects.AddEffect(new WeaknessDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists, 1));
            // targetHitEffects.AddEffect(new ApplyShatterEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists));
            
            Hitbox.TargetHitEffects = targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
            Hitbox.HolderHitEffects = holderHitEffects;
        }

        private void AddHitboxCollision()
        {
            Circle = new Circle
            {
                Radius                  = HitboxRadius,
                Visible                 = false,
                IgnoresParentVisibility = true,
                RelativeX               = InitialDistance,
                RelativeY               = PerpendicularOffset,
            };

            Circle.AttachTo(Hitbox);
            Hitbox.Collision.Add(Circle);
        }

        private void ConfigureHitboxSprite()
        {
            Hitbox.SpriteInstance.CurrentChainName             = "Finisher";
            Hitbox.SpriteInstance.AnimationSpeed               = 0.99f / (float)ActiveDuration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeX                    = InitialDistance;
            Hitbox.SpriteInstance.RelativeY                    = PerpendicularOffset;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
