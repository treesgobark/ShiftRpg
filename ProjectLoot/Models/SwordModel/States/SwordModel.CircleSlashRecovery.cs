using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class CircleSlashRecovery : ParentedTimedState<SwordModel>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(240);
        
        private IState? NextState { get; set; }
        
        public CircleSlashRecovery(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(timeManager, parent)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
            NextState       = null;
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                NextState = _states.Get<Slash1>();
            }

            if (TimeInState >= Duration)
            {
                if (!Parent.IsEquipped)
                {
                    return _states.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }
                
                return _states.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate() { }
    }
}