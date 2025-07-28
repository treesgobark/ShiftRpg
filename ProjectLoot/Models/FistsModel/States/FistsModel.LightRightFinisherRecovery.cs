using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class LightRightFinisherRecovery : ParentedTimedState<FistsModel>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(360);

        public LightRightFinisherRecovery(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(timeManager, weaponModel)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return _states.Get<NotEquipped>();
            }
            
            if (TimeInState >= Duration)
            {
                return _states.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate()
        {
        }
    }
}
