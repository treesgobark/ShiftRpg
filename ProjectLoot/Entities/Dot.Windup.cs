using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Windup : ParentedTimedState<Dot>
    {
        private readonly IReadonlyStateMachine _states;
        private static Rotation RotationPerSecond => Rotation.FullTurn;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(1000);
        
        public Windup(IReadonlyStateMachine states, ITimeManager timeManager, Dot parent) : base(timeManager, parent)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
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
                return _states.Get<Attacking>();
            }
            
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.SatelliteSprite.CurrentChainName  =  Parent.IsBig ? "BigBlueSquares" : "BlueSquares";
            Parent.SatelliteSprite.FlipHorizontal    =  false;
            Parent.SatelliteSprite.RelativeRotationZ += (float)TimeManager.GameTimeSinceLastFrame.TotalSeconds
                                                        * RotationPerSecond.TotalRadians;
        }

        public override void BeforeDeactivate()
        {
        }
    }
}