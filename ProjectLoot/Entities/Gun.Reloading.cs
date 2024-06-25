using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Gun
{
    protected class Reloading : ParentedTimedState<Gun>
    {
        public Reloading(Gun parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override void OnActivate()
        {
            Parent.StartReload();
            base.OnActivate();
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.ReloadProgress = (float)(TimeInState / Parent.ReloadTime);
        }
    
        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.ReloadTime)
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
