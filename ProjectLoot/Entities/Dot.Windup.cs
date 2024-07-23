using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Dot
{
    private class Windup : ParentedTimedState<Dot>
    {
        private static Rotation RotationPerSecond => Rotation.FullTurn;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(1000);
        
        public Windup(IReadonlyStateMachine states, ITimeManager timeManager, Dot parent) : base(states, timeManager, parent)
        {
        }

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate()
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.PoiseComponent.IsAboveThreshold)
            {
                Parent.PoiseComponent.CurrentPoiseDamage = 0;
                
                return States.Get<Idle>();
            }
            
            if (TimeInState >= Duration)
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