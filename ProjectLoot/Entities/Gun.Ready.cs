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
        
        protected IState? NextState { get; set; }
    
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
        
        protected override void AfterTimedStateActivity()
        {
            if (Parent.InputDevice.Fire.WasJustPressed || Parent is { FiringType: FiringType.Automatic, InputDevice.Fire.IsDown: true })
            {
                FireBullet();
                NextState = StateMachine.Get<Recovery>();
            }
    
            if (Parent.InputDevice.Reload.WasJustPressed)
            {
                NextState = StateMachine.Get<Reloading>();
            }
        }
    
        public override IState? EvaluateExitConditions()
        {
            if (Parent.MagazineRemaining <= 0)
            {
                return StateMachine.Get<Reloading>();
            }
            
            if (NextState is not null)
            {
                return NextState;
            }
    
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            NextState = null;
        }

        public override void Uninitialize() { }

        private void FireBullet()
        {
            Parent.Fire();
        }
    }
}
