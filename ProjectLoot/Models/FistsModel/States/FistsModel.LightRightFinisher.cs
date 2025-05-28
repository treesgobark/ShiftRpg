using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;
using ToolsUtilitiesStandard.Helpers;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class LightRightFinisher : ParentedTimedState<FistsModel>
    {
        private static TimeSpan SwingDuration => TimeSpan.FromMilliseconds(60);
        private static TimeSpan TotalDuration => TimeSpan.FromMilliseconds(120);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(150);
        private float NormalizedSwingProgress => (float)Math.Clamp(TimeInState / SwingDuration, 0, 1);
        private float NormalizedProgress => (float)Math.Clamp(TimeInState      / TotalDuration, 0, 1);
        private static float HitboxRadius => 24;
        private static float PerpendicularOffset => -4;
        private static float TravelDistance => 24;
        private static float InitialDistance => -4;
        private static float Damage => 16;

        private MeleeHitbox? Hitbox { get; set; }
        private Circle? Circle { get; set; }
        private Rotation AttackDirection { get; set; }
        private float ZOffset { get; set; }
        
        private IState? NextState { get; set; }
        
        public LightRightFinisher(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            NextState = null;

            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

            CalculateZOffset();
            CreateHitbox();
            AddTargetHitEffects();
            AddHitboxCollision();
            ConfigureHitboxSprite();

            GlobalContent.SwingA.Play(0.1f, 0, 0);
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState >= TotalDuration)
            {
                if (!Parent.IsEquipped)
                {
                    return States.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }

                return States.Get<LightRightFinisherRecovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.SpriteInstance.RelativeX = InitialDistance + MathHelper.SmoothStep(0, 1, NormalizedSwingProgress) * TravelDistance;
            Hitbox.SpriteInstance.Alpha = 1f - NormalizedProgress;
            Circle.RelativeX                = InitialDistance + MathF.Pow(NormalizedSwingProgress, 4) * TravelDistance;
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
                    700,
                    AttackDirection,
                    KnockbackBehavior.Replacement
                )
            );
                
            targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
            
            // targetHitEffects.AddEffect(new WeaknessDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists, 1));
            // targetHitEffects.AddEffect(new ApplyShatterEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists));
            
            Hitbox.TargetHitEffects = targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
            Hitbox.HolderHitEffects = holderHitEffects;
        }

        private void AddHitboxCollision()
        {
            Circle = new()
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
            Hitbox.SpriteInstance.AnimationSpeed               = 0.99f / (float)SwingDuration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeX                    = InitialDistance;
            Hitbox.SpriteInstance.RelativeY                    = PerpendicularOffset;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
