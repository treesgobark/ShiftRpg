using ANLG.Utilities.FlatRedBall.States;

namespace ShiftRpg.Entities;

public partial class MeleeWeapon
{
    protected class Startup : TimedState<MeleeWeapon>
    {
        public Startup(MeleeWeapon parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            // Parent.Owner.SetPlayerColor(Color.Yellow);
            Parent.Holder.SetInputEnabled(false);
        }

        public override void CustomActivity()
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.StartupTime)
            {
                return StateMachine.Get<Active>();
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