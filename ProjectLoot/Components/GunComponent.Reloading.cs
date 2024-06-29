using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Components;

public partial class GunComponent
{
    protected class Reloading : ParentedTimedState<GunComponent>
    {
        public Reloading(GunComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentGun.GunData.ReloadTimeSpan)
            {
                Parent.CurrentGun.CurrentRoundsInMagazine = Parent.CurrentGun.GunData.MagazineSize;
                Parent.CurrentMagazineCount               = Parent.CurrentGun.GunData.MagazineSize;
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
