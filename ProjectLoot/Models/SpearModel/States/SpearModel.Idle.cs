using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class Idle : ParentedTimedState<SpearModel>
    {
        private readonly IReadonlyStateMachine _states;

        public Idle(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(timeManager, weaponModel)
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
                return _states.Get<Thrust>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return _states.Get<Toss>();
            }

            return null;
        }

        public override void BeforeDeactivate() { }
    }
}
