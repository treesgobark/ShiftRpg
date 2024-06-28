using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Gun
{
    protected class Ready : ParentedTimedState<Gun>
    {
        public Ready(Gun parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
        
        protected override void AfterTimedStateActivity() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (Parent.GunModel is null)
            {
                return null;
            }
            
            if (Parent.GunModel is { CurrentRoundsInMagazine: <= 0 } || Parent.InputDevice.Reload.WasJustPressed)
            {
                return StateMachine.Get<Reloading>();
            }
            
            if (Parent.InputDevice.Fire.IsDown)
            {
                FireBullet();
                return StateMachine.Get<Recovery>();
            }
    
            return null;
        }
    
        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }

        private void FireBullet()
        {
            Parent.Fire();
        }
    }
}
