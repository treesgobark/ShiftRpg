using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Idle : ParentedTimedState<MeleeWeapon>
    {
        public Idle(MeleeWeapon parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(parent, stateMachine, timeManager)
        {
        }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.InputDevice.Attack.WasJustPressed)
            {
                return StateMachine.Get<Startup>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    }
}
