using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Models;

public partial class StandardGunModel
{
    private class Recovery : TimedState
    {
        private readonly IReadonlyStateMachine _states;
        private StandardGunModel GunModel { get; }
        
        public Recovery(IReadonlyStateMachine states, ITimeManager timeManager, StandardGunModel gunModel) : base(timeManager)
        {
            _states = states;
            GunModel     = gunModel;
        }

        private IState? NextState { get; set; }
        
        protected override void AfterTimedStateActivate() { }
    
        protected override void AfterTimedStateActivity()
        {
            if (GunModel.GunComponent.GunInputDevice.Reload.WasJustPressed)
            {
                NextState = _states.Get<Reloading>();
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
                return _states.Get<Ready>();
            }
    
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            NextState = null;
        }
    }
}
