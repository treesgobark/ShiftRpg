using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.States;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Factories;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Attacking : ParentedTimedState<Dot>
    {
        private readonly IReadonlyStateMachine _states;
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
        
        public Attacking(IReadonlyStateMachine states, ITimeManager timeManager, Dot parent) : base(timeManager, parent)
        {
            _states = states;
        }

        private MeleeHitbox Hitbox { get; set; }

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
                
                return _states.Get<Idle>();
            }

            if (TimeInState >= Duration)
            {
                return _states.Get<Idle>();
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

        public override void BeforeDeactivate()
        {
            Hitbox.Destroy();
        }
    }
}