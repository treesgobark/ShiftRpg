using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class NotEquipped : ParentedTimedState<SwordModel>
    {
        private readonly IReadonlyStateMachine _states;

        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel)
            : base(timeManager, swordModel)
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
