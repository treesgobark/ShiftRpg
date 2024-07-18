using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;

namespace ProjectLoot.Models;

public partial class StandardGunModel
{
    private class Recovery : TimedState
    {
        private StandardGunModel GunModel { get; }
        
        public Recovery(IReadonlyStateMachine stateMachine, ITimeManager timeManager, StandardGunModel gunModel) : base(stateMachine, timeManager)
        {
            GunModel  = gunModel;
        }

        private IState? NextState { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        protected override void AfterTimedStateActivity()
        {
            if (GunModel.GunComponent.GunInputDevice.Reload.WasJustPressed)
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
    
            if (TimeInState > GunModel.GunData.TimePerRound)
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
