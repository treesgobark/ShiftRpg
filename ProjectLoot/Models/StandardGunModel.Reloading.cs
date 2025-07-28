using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Models;

public partial class StandardGunModel
{
    private class Reloading : TimedState
    {
        private readonly IReadonlyStateMachine _states;
        private StandardGunModel GunModel { get; }

        public Reloading(IReadonlyStateMachine states, ITimeManager  timeManager, StandardGunModel gunModel)
            : base(timeManager)
        {
            _states = states;
            GunModel     = gunModel;
        }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (!GunModel.IsEquipped)
            {
                return _states.Get<NotEquipped>();
            }
            
            if (TimeInState > GunModel.GunData.ReloadTimeSpan)
            {
                GunModel.CurrentRoundsInMagazine           = GunModel.GunData.MagazineSize;
                GunModel.GunViewModel.CurrentMagazineCount = GunModel.GunData.MagazineSize;
                return _states.Get<Ready>();
            }
    
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }
    
        public override void BeforeDeactivate()
        {
        }
    }
}
