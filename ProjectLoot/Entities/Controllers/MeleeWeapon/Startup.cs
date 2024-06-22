using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

public partial class MeleeWeapon
{
    protected class Startup : TimedState<IMeleeWeapon>
    {
        public Startup(IMeleeWeapon parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Holder.InputEnabled = false;
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
            Parent.Holder.InputEnabled = true;
        }

        public override void Uninitialize() { }
    }
}