using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class Slash2Recovery : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(360);
        
        public Slash2Recovery(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState) { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                return States.Get<Slash3>();
            }
            
            if (TimeInState >= Duration)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}