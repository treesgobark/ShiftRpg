using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class Idle : ParentedTimedState<SwordModel>
    {
        private readonly IReadonlyStateMachine _states;

        public Idle(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel)
            : base(timeManager, swordModel)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return _states.Get<NotEquipped>();
            }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                return _states.Get<Slash1>();
            }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return _states.Get<Spin>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }
    }
}
