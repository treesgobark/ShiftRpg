using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

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

        protected override void AfterTimedStateActivity()
        {
            Parent.ReloadProgress = (float)(TimeInState / Parent.ReloadTime.TotalSeconds);
        }
    
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
