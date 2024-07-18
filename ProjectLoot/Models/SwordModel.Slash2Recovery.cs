using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class Slash2Recovery : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(180);
        
        public Slash2Recovery(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel parent)
            : base(stateMachine, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return StateMachine.Get<NotEquipped>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                return StateMachine.Get<Slash3>();
            }
            
            if (TimeInState >= Duration)
            {
                return StateMachine.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    }
}