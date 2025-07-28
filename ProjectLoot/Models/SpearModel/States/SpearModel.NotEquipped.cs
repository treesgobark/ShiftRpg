using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class NotEquipped : ParentedTimedState<SpearModel>
    {
        private readonly IReadonlyStateMachine _states;

        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(timeManager, weaponModel)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.IsEquipped)
            {
                return _states.Get<Idle>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }
    }
}
