using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class LightLeftHookRecovery : ParentedTimedState<FistsModel>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(360);

        public LightLeftHookRecovery(IReadonlyStateMachine states, ITimeManager timeManager, FistsModel weaponModel)
            : base(timeManager, weaponModel)
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
                return _states.Get<LightRightFinisher>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return _states.Get<HeavyRightJab>();
            }
            
            if (TimeInState >= Duration)
            {
                return _states.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate()
        {
        }
    }
}
