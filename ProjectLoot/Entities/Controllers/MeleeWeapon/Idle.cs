using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Idle : TimedState<IMeleeWeapon>
    {
        public Idle(IMeleeWeapon parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine)
        {
        }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        public override void CustomActivity() { }

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
