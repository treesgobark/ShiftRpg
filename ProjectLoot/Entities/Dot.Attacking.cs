using ANLG.Utilities.Core.Extensions;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Utilities;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Factories;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Attacking : ParentedTimedState<Dot>
    {
        private Rotation RotationPerSecond => 3 * Rotation.FullTurn;
        private float AttackVelocity => 800;
        private Vector3 ToTargetDirection => Parent.Position.GetVectorTo(Parent.Target.Position)
                                                   .Scale(z: 0)
                                                   .GetNormalized();

        private TimeSpan Duration => TimeSpan.FromMilliseconds(500);
        
        private int Swooshes => 6;
        private int SwooshesPlayed { get; set; }
        private int GoalSwooshes => Math.Clamp((int)(NormalizedProgress * Swooshes) + 1, 0, Swooshes);
        
        private float NormalizedProgress => (float)(TimeInState / Duration);
        
        public Attacking(IReadonlyStateMachine states, ITimeManager timeManager, Dot parent) : base(states, timeManager, parent)
        {
        }

        private MeleeHitbox Hitbox { get; set; }

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate()
        {
            SwooshesPlayed = 0;
            
            Parent.SatelliteSprite.CurrentChainName = Parent.IsBig ? "BigBlueSquaresSmear" : "BlueSquaresSmear";
            Parent.SatelliteSprite.FlipHorizontal   = true;

            Hitbox = MeleeHitboxFactory.CreateNew();
            
            Circle circle = new()
            {
                Radius = Parent.IsBig ? 20 : 10,
                Visible = false,
            };
            circle.AttachTo(Hitbox);
            Hitbox.Collision.Add(circle);
            
            Hitbox.AttachTo(Parent);
            Hitbox.SpriteInstance.Visible = false;
            Hitbox.AppliesTo              = Team.Player;

            EffectBundle targetHitEffects = new();
            targetHitEffects.AddEffect(new AttackEffect(~Parent.Effects.Team, SourceTag.Sword, 20));
            targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~Parent.Effects.Team,
                    SourceTag.None,
                    250,
                    Rotation.FromRadians(ToTargetDirection.AngleOrZero()),
                    KnockbackBehavior.Replacement)
                );
            Hitbox.TargetHitEffects = targetHitEffects;

            Hitbox.HolderEffectsComponent = Parent.Effects;

            Parent.Velocity = ToTargetDirection * AttackVelocity;
            
            Parent.ForceUpdateDependenciesDeep();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.Poise.IsAboveThreshold)
            {
                Parent.Poise.CurrentPoiseDamage = 0;
                
                return States.Get<Idle>();
            }

            if (TimeInState >= Duration)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.SatelliteSprite.RelativeRotationZ += (float)TimeManager.GameTimeSinceLastFrame.TotalSeconds
                                                        * RotationPerSecond.TotalRadians;

            PlaySounds();
        }

        private void PlaySounds()
        {
            if (SwooshesPlayed >= GoalSwooshes)
            {
                return;
            }

            float pitch = Random.Shared.NextSingle(-0.5f, 0.5f);

            switch (SwooshesPlayed % 3)
            {
                case 0:
                    GlobalContent.WhooshA.Play(0.1f, pitch, 0);
                    break;
                case 1:
                    GlobalContent.WhooshB.Play(0.1f, pitch, 0);
                    break;
                case 2:
                    GlobalContent.WhooshC.Play(0.1f, pitch, 0);
                    break;
            }

            SwooshesPlayed++;
        }

        public override void BeforeDeactivate(IState? nextState)
        {
            Hitbox.Destroy();
        }

        public override void Uninitialize()
        {
        }
    }
}