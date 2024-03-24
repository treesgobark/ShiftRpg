using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Entities;

partial class MeleeWeapon
{
    public class Recovery : TimedState<MeleeWeapon>
    {
        public Recovery(MeleeWeapon parent, IStateMachine stateMachine) : base(parent, stateMachine) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Owner.SetPlayerColor(Color.Blue);
            Parent.Owner.InputEnabled = false;
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
            Parent.Owner.InputEnabled = true;
        }
    }
}