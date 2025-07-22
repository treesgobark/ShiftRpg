using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class NotEquipped : ParentedTimedState<SpearModel>
    {
        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
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
