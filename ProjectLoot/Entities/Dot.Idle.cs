using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Idle : ParentedTimedState<Dot>
    {
        private Rotation RotationPerSecond => Rotation.EighthTurn;
        private TimeSpan Duration => TimeSpan.FromMilliseconds(1500);
        private TimeSpan RandomDuration =>
            Duration + Duration * MathHelper.Lerp(-DurationTolerance, DurationTolerance, RandomizedTValue);
        private float DurationTolerance => 0.3f;
        private float RandomizedTValue { get; set; }
        
        public Idle(IReadonlyStateMachine states, ITimeManager timeManager, Dot parent) : base(states, timeManager, parent)
        {
        }

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            RandomizedTValue = Random.Shared.NextSingle();
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState >= RandomDuration)
            {
                return States.Get<Windup>();
            }
            
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.Poise.IsAboveThreshold)
            {
                Parent.Poise.CurrentPoiseDamage = 0;
                TimeInState                              = TimeSpan.Zero;
            }
            
            Parent.CorpseInformationComponent.BodyChainName = Parent.IsBig ? "BigBlueIdle" : "BlueIdle";
            
            Parent.SatelliteSprite.CurrentChainName         = Parent.IsBig ? "BigBlueSquares" : "BlueSquares";
            Parent.SatelliteSprite.FlipHorizontal           = false;
            Parent.SatelliteSprite.RelativeRotationZ += (float)TimeManager.GameTimeSinceLastFrame.TotalSeconds
                                                        * RotationPerSecond.TotalRadians;
            
            Parent.BodySprite.CurrentChainName = Parent.IsBig ? "BigBlueIdle" : "BlueIdle";
            
            Parent.CircleInstance.Radius = Parent.IsBig ? 16 : 8;
        }

        public override void BeforeDeactivate(IState? nextState)
        {
        }

        public override void Uninitialize()
        {
        }
    }
}