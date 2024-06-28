using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Gun
{
    protected class Recovery : ParentedTimedState<Gun>
    {
        public Recovery(Gun parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }

        private IState? NextState { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        protected override void AfterTimedStateActivity()
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
    
            if (TimeInState > Parent.GunModel.GunData.TimePerRound)
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
