using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Recovery : ParentedTimedState<MeleeWeapon>
    {
        public Recovery(MeleeWeapon parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.RecoveryTimeSpan)
            {
                return StateMachine.Get<Idle>();
            }

            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.Holder.SetInputEnabled(true);
        }

        public override void Uninitialize() { }
    }
}