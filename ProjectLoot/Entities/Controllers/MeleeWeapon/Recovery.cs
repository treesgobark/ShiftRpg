using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Recovery : TimedState<IMeleeWeapon>
    {
        public Recovery(IMeleeWeapon parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.RecoveryTime)
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