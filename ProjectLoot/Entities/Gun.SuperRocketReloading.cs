using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

public partial class Gun
{
    protected class SuperRocketReloading : Reloading
    {
        public SuperRocketReloading(Gun parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Fire();
            Parent.StartReload();
        }
    
        public override void CustomActivity() { }
    
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
    }
}
