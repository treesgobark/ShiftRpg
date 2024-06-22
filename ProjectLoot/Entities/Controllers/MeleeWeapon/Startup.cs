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
            Parent.Holder.SetInputEnabled(false);
        }

        protected override void AfterTimedStateActivity()
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

        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    }
}