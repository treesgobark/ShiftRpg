using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Recovery : TimedState<IMeleeWeapon>
    {
        public Recovery(IMeleeWeapon parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            // Parent.Owner.SetPlayerColor(Color.Blue);
            Parent.Holder.InputEnabled = false;
        }

        public override void CustomActivity() { }

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
            Parent.Holder.InputEnabled = true;
        }

        public override void Uninitialize() { }
    }
}