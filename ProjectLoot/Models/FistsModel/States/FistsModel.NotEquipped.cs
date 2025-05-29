using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class NotEquipped : ParentedTimedState<FistsModel>
    {
        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
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
