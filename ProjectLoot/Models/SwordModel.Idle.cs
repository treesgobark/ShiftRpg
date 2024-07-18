using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class Idle : ParentedTimedState<SwordModel>
    {
        public Idle(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel swordModel)
            : base(stateMachine, timeManager, swordModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return StateMachine.Get<NotEquipped>();
            }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                return StateMachine.Get<Slash1>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    }
}
