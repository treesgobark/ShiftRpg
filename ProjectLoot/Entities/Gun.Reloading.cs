using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Gun
{
    protected class Reloading : ParentedTimedState<Gun>
    {
        public Reloading(Gun parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.GunModel.GunData.ReloadTimeSpan)
            {
                Parent.GunModel.CurrentRoundsInMagazine = Parent.GunModel.GunData.MagazineSize;
                return StateMachine.Get<Ready>();
            }
    
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }
    
        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize() { }
    }
}
