using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class Idle : ParentedTimedState<SwordModel>
    {
        public Idle(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel)
            : base(states, timeManager, swordModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                return States.Get<Slash1>();
            }

            return null;
        }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}
