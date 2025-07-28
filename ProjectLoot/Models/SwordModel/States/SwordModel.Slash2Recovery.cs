using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class Slash2Recovery : ParentedTimedState<SwordModel>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(360);
        
        public Slash2Recovery(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(timeManager, parent)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return _states.Get<NotEquipped>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                return _states.Get<Slash3>();
            }
            
            if (TimeInState >= Duration)
            {
                return _states.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate() { }
    }
}