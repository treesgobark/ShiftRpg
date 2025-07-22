using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class Idle : ParentedTimedState<FistsModel>
    {
        public Idle(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState) { }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                return States.Get<LightRightJab>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return States.Get<HeavyRightJab>();
            }

            return null;
        }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}
