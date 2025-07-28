using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class Thrust : ParentedTimedState<SpearModel>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan SwingDuration => TimeSpan.FromMilliseconds(60);
        private static TimeSpan TotalDuration => TimeSpan.FromMilliseconds(180);
        private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(75);
        private float NormalizedSwingProgress => (float)Math.Clamp(TimeInState / SwingDuration, 0, 1);
        private float NormalizedProgress => (float)Math.Clamp(TimeInState      / TotalDuration, 0, 1);
        private static float HitboxRadius => 4;
        private static float PerpendicularOffset => -4;
        private static float TravelDistance => 48;
        private static float InitialDistance => 4;
        private static float HitboxSpriteOffset => -24;
        private static float Damage => 18;

        private MeleeHitbox? Hitbox { get; set; }
        private Circle? Circle { get; set; }
        private Rotation AttackDirection { get; set; }
        private float ZOffset { get; set; }
        private EffectBundle _targetHitEffects;
        
        private IState? NextState { get; set; }
        
        public Thrust(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(timeManager, weaponModel)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
            NextState = null;

            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

            CalculateZOffset();
            CreateHitbox();
            AddHitboxCollision();
            ConfigureHitboxSprite();
            AddTargetHitEffects();

            GlobalContent.SwingA.Play(0.1f, 0, 0);
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                NextState = _states.Get<Thrust>();
            }
            
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                NextState = _states.Get<Toss>();
            }
            
            if (TimeInState >= TotalDuration)
            {
                if (!Parent.IsEquipped)
                {
                    return _states.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }

                return _states.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Hitbox.SpriteInstance.RelativeX = InitialDistance + NormalizedSwingProgress * TravelDistance + HitboxSpriteOffset;
            Hitbox.SpriteInstance.Alpha     = 1f              - NormalizedProgress;
            Circle.RelativeX                = InitialDistance + NormalizedSwingProgress * TravelDistance;
            
            UpdateTargetHitEffects();
        }

        private void UpdateTargetHitEffects()
        {
            _targetHitEffects.UpsertEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Spear,
                    float.Lerp(800, 400, NormalizedSwingProgress), 
                    AttackDirection,
                    KnockbackBehavior.Replacement
                )
            );
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
        }

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
            _targetHitEffects = new EffectBundle();
            
            _targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, Damage));
                    
            _targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear,
                                                         HitstopDuration));

            // targetHitEffects.AddEffect(
            //     new KnockTowardEffect
            //     {
            //         AppliesTo = ~Parent.MeleeWeaponComponent.Team,
            //         Source = SourceTag.Spear,
            //         Duration = Parent.KnockTowardDuration,
            //         TargetPosition = Parent.MeleeWeaponComponent.HolderGameplayCenterPosition.AtZ(0)
            //                          + Vector2Extensions.FromAngleAndLength(AttackDirection.NormalizedRadians, Parent.KnockTowardDistance).ToVector3(),
            //     }
            // );
                
            _targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, 10));
            
            // targetHitEffects.AddEffect(new WeaknessDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear, 1));
            // targetHitEffects.AddEffect(new ApplyShatterEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Spear));
            
            Hitbox.TargetHitEffects = _targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Spear, HitstopDuration));
        
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
            Hitbox.SpriteInstance.CurrentChainName = "SpearThrust";
            Hitbox.SpriteInstance.AnimationSpeed   = 0.99f / (float)SwingDuration.TotalSeconds;
            Hitbox.SpriteInstance.RelativeX        = InitialDistance + HitboxSpriteOffset;
            Hitbox.SpriteInstance.RelativeY        = PerpendicularOffset;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
