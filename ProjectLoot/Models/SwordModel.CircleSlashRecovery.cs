using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models;

public partial class SwordModel
{
    private class CircleSlashRecovery : ParentedTimedState<SwordModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(240);
        
        private IState? NextState { get; set; }
        
        public CircleSlashRecovery(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
            : base(states, timeManager, parent) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            NextState       = null;
        }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            {
                NextState = States.Get<Slash1>();
            }

            if (TimeInState >= Duration)
            {
                if (!Parent.IsEquipped)
                {
                    return States.Get<NotEquipped>();
                }

                if (NextState is not null)
                {
                    return NextState;
                }
                
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}