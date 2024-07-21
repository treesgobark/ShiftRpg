using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Idle : ParentedTimedState<Dot>
    {
        private Rotation RotationPerSecond => Rotation.EighthTurn;
        private TimeSpan Duration => TimeSpan.FromSeconds(2);
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

        protected override void AfterTimedStateActivate()
        {
            RandomizedTValue = Random.Shared.NextSingle();
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState >= RandomDuration)
            {
                return States.Get<Attacking>();
            }
            
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.SatelliteSprite.CurrentChainName  =  "BlueSquares";
            Parent.SatelliteSprite.FlipHorizontal    =  false;
            Parent.SatelliteSprite.RelativeRotationZ += (float)TimeManager.GameTimeSinceLastFrame.TotalSeconds
                                                        * RotationPerSecond.TotalRadians;
        }

        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize()
        {
        }
    }
}