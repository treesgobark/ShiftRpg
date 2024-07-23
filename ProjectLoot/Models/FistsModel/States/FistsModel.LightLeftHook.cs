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

partial class FistsModel
{
    private class LightLeftHook : ParentedTimedState<FistsModel>
    {
        private static TimeSpan SwingDuration => TimeSpan.FromMilliseconds(60);
        private static TimeSpan TotalDuration => TimeSpan.FromMilliseconds(120);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(50);
        private float NormalizedSwingProgress => (float)Math.Clamp(TimeInState / SwingDuration, 0, 1);
        private static float HitboxRadius => 10;
        private static float PerpendicularOffset => -8;
        private static float ForwardOffset => 6;
        private static float Damage => 8;
        

        private MeleeHitbox? Hitbox { get; set; }
        private Circle? Circle { get; set; }
        private Rotation AttackDirection { get; set; }
        private Rotation HitboxStartDirection => AttackDirection + Rotation.QuarterTurn;
        private float ZOffset { get; set; }
        
        private IState? NextState { get; set; }
        
        public LightLeftHook(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
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

            GlobalContent.SwingB.Play(0.1f, 0, 0);
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                NextState = States.Get<LightRightFinisher>();
            }
            
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                NextState = States.Get<HeavyRightJab>();
            }

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

                return States.Get<LightLeftHookRecovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.RelativeRotationZ =
                (HitboxStartDirection - Rotation.EighthTurn * NormalizedSwingProgress).NormalizedRadians;
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

        public override void Uninitialize() { }

        private void CalculateZOffset()
        {
            int sector = AttackDirection.GetSector(8, true);
            ZOffset = sector switch { 6 or 5 or 4 => 0.2f, 3 or 2 or 1 or 0 or 7 => -0.2f, _ => ZOffset };
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
            
            targetHitEffects.AddEffect(new DamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists, Damage));
                    
            targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Fists,
                                                         HitstopDuration));

            targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Fists,
                    200,
                    AttackDirection - Rotation.EighthTurn / 2,
                    KnockbackBehavior.Replacement
                )
            );
                
            targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 7));
            
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
                RelativeX               = ForwardOffset,
                RelativeY               = PerpendicularOffset,
            };

            Circle.AttachTo(Hitbox);
            Hitbox.Collision.Add(Circle);
        }

        private void ConfigureHitboxSprite()
        {
            Hitbox.SpriteInstance.CurrentChainName              = "Hook";
            Hitbox.SpriteInstance.AnimationSpeed                = 0.99f / (float)SwingDuration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeX                     = ForwardOffset;
            Hitbox.SpriteInstance.RelativeY                     = PerpendicularOffset;
            Hitbox.SpriteInstance.RelativeRotationZ             = -Rotation.QuarterTurn.NormalizedRadians;
            Hitbox.SpriteInstance.FlipVertical                  = true;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
