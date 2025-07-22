using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class NotEquipped : ParentedTimedState<SwordModel>
    {
        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel)
            : base(states, timeManager, swordModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState) { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.IsEquipped)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}
