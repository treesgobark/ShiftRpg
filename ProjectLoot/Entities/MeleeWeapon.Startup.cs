using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class MeleeWeapon
{
    protected class Startup : ParentedTimedState<MeleeWeapon>
    {
        public Startup(MeleeWeapon parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Holder.SetInputEnabled(false);
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.StartupTimeSpan)
            {
                return StateMachine.Get<Active>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    }
}