using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;

namespace ProjectLoot.Models;

public partial class StandardGunModel
{
    private class Reloading : TimedState
    {
        private StandardGunModel GunModel { get; }

        public Reloading(IReadonlyStateMachine states, ITimeManager  timeManager, StandardGunModel gunModel)
            : base(states, timeManager)
        {
            GunModel  = gunModel;
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (!GunModel.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }
            
            if (TimeInState > GunModel.GunData.ReloadTimeSpan)
            {
                GunModel.CurrentRoundsInMagazine           = GunModel.GunData.MagazineSize;
                GunModel.GunViewModel.CurrentMagazineCount = GunModel.GunData.MagazineSize;
                return States.Get<Ready>();
            }
    
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }
    
        public override void BeforeDeactivate(IState? nextState)
        {
        }

        public override void Uninitialize() { }
    }
}
