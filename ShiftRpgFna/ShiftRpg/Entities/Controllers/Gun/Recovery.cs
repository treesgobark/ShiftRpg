using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.States;
using ShiftRpg.Contracts;
using ShiftRpg.Effects.Handlers;

namespace ShiftRpg.Entities;

public partial class Gun
{
    protected class Recovery : TimedState<IGun>
    {
        public Recovery(IGun parent, IStateMachine stateMachine) : base(parent, stateMachine) { }

        private IState? NextState { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override void CustomActivity()
        {
            if (Parent.InputDevice.Reload.WasJustPressed)
            {
                NextState = StateMachine.Get<Reloading>();
            }
        }
    
        public override IState? EvaluateExitConditions()
        {
            if (NextState is not null)
            {
                return NextState;
            }
    
            if (TimeInState > Parent.TimePerRound.TotalSeconds)
            {
                return StateMachine.Get<Ready>();
            }
    
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            NextState = null;
        }

        public override void Uninitialize() { }
    }
}
