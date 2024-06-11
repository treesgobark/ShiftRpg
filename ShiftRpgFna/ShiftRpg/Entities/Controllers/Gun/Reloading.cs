using ANLG.Utilities.FlatRedBall.States;
using ShiftRpg.Contracts;

namespace ShiftRpg.Entities;

public partial class Gun
{
    protected class Reloading : TimedState<IGun>
    {
        public Reloading(IGun parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override void OnActivate()
        {
            Parent.StartReload();
            base.OnActivate();
        }
    
        public override void CustomActivity() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.ReloadTime.TotalSeconds)
            {
                return StateMachine.Get<Ready>();
            }
    
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            Parent.FillMagazine();
        }

        public override void Uninitialize() { }
    }
}
