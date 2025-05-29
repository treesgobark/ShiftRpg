using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class LightLeftHookRecovery : ParentedTimedState<FistsModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(360);

        public LightLeftHookRecovery(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
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
                return States.Get<LightRightFinisher>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return States.Get<HeavyRightJab>();
            }
            
            if (TimeInState >= Duration)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate(IState? nextState)
        {
        }

        public override void Uninitialize() { }
    }
}
