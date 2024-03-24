using ANLG.Utilities.FlatRedBall.States;
using ShiftRpg.Contracts;

namespace ShiftRpg.Entities;

public partial class Gun
{
    protected class Ready : TimedState<IGun>
    {
        public Ready(IGun parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        protected IState? NextState { get; set; }
    
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override void CustomActivity()
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
    
        private void FireBullet()
        {
            Parent.Fire();
        }
    }
}
