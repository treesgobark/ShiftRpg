using ANLG.Utilities.FlatRedBall.States;
using ShiftRpg.Contracts;

namespace ShiftRpg.Entities;

public partial class Gun
{
    protected class SuperRocketReloading : Reloading
    {
        public SuperRocketReloading(IGun parent, IStateMachine stateMachine) : base(parent, stateMachine) { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Fire();
            Parent.StartReload();
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
    }
}
