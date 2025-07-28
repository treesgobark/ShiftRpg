using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Controllers;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class Toss : ParentedTimedState<SpearModel>
    {
        private readonly IReadonlyStateMachine _states;
        private readonly ITimeManager _timeManager;
        private StateMachine _stateMachine;
        
        public Toss(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(timeManager, weaponModel)
        {
            _states       = states;
            _timeManager  = timeManager;
            
            _stateMachine = new StateMachine();
            
            _stateMachine.Add(new TossWindup(_stateMachine, _timeManager, this));
            _stateMachine.Add(new TossActive(_stateMachine, _timeManager, this));
            _stateMachine.Add(new TossedSpearInGround(_stateMachine, _timeManager, this));
            _stateMachine.Add(new TossRecall(_stateMachine, _timeManager, this));
        }

        public Rotation AttackDirection { get; set; }
        public IMeleeWeaponComponent MeleeWeaponComponent => Parent.MeleeWeaponComponent;
        public MeleeHitbox Hitbox { get; set; }
        public float ChargeProgress { get; set; }
        
        private static float CircleRadius => 4;
        private static int CircleCount => 5;
        private static float TotalCircleLength => 48f;
        private static float DistanceBetweenCircles =>
            CircleCount > 1 ? TotalCircleLength / (CircleCount - 1) : TotalCircleLength;
        
        private static float HitboxSpriteOffset => -24;
        
        private float ZOffset { get; set; }

        protected override void AfterTimedStateActivate()
        {
            _stateMachine.SetStartingState<TossWindup>();
            _stateMachine.AdvanceCurrentState();
            
            AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

            CalculateZOffset();
            CreateHitbox();
            AddHitboxCollision();
            ConfigureHitboxSprite();
        }

        protected override void AfterTimedStateActivity()
        {
            _stateMachine.DoCurrentStateActivity();
        }

        public override IState? EvaluateExitConditions()
        {
            if (!_stateMachine.IsRunning)
            {
                return _states.Get<Idle>();
            }

            return null;
        }

        public override void BeforeDeactivate()
        {
            Hitbox?.Destroy();
            _stateMachine.ShutDown();
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
            Hitbox.IsActive                      = false;

            Hitbox.HolderEffectsComponent = Parent.HolderEffects;
            Hitbox.AppliesTo              = ~Parent.MeleeWeaponComponent.Team;
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

                circle.AttachTo(Hitbox);
                Hitbox.Collision.Add(circle);
            }
        }

        private void ConfigureHitboxSprite()
        {
            Hitbox.SpriteInstance.CurrentChainName = "SpearThrust";
            Hitbox.SpriteInstance.RelativeX        = HitboxSpriteOffset;

            Hitbox.SpriteInstance.RelativeZ = Parent.MeleeWeaponComponent.HolderSpritePosition.Z + ZOffset;
        }
    }
}
